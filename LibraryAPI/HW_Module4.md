# Домашнє завдання. Модуль 4: DTO та AutoMapper

## 1. Базовий стан проєкту з заняття (для тих, хто пропустив)

Якщо ви були відсутні на лекції, для виконання домашнього завдання вам необхідно створити стартовий проєкт `LibraryApi` з нуля. 

### Крок 1. Створення та конфігурація проєкту
1. Створіть новий Web API проєкт у вашому терміналі:
   ```bash
   dotnet new webapi -n LibraryApi --use-controllers
   ```
2. Перейдіть до папки проєкту та встановіть необхідні NuGet-пакети:
   ```bash
   dotnet add package AutoMapper
   dotnet add package Scalar.AspNetCore
   dotnet add package Swashbuckle.AspNetCore.SwaggerUI
   ```

### Крок 2. Кодова база заняття
Створіть наступні класи у відповідних папках вашого проєкту:

#### Models/Book.cs
```csharp
namespace LibraryApi.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public int PublishedYear { get; set; }
    public decimal PurchasePrice { get; set; }
    public string InternalNotes { get; set; } = string.Empty;
}
```

#### Dtos/BookDto.cs
```csharp
namespace LibraryApi.Dtos;

public class BookDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public int PublishedYear { get; set; }
}
```

#### Dtos/CreateBookRequest.cs
```csharp
using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Dtos;

public class CreateBookRequest
{
    [Required(ErrorMessage = "Назва книги є обов'язковою.")]
    [MaxLength(100, ErrorMessage = "Назва книги не може перевищувати 100 символів.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Автор є обов'язковим.")]
    [MaxLength(50, ErrorMessage = "Ім'я автора не може перевищувати 50 символів.")]
    public string Author { get; set; } = string.Empty;

    [Required(ErrorMessage = "Жанр є обов'язковим.")]
    [MaxLength(30, ErrorMessage = "Назва жанру не може перевищувати 30 символів.")]
    public string Genre { get; set; } = string.Empty;

    [Range(1000, 2100, ErrorMessage = "Рік видання має бути між 1000 та 2100 роками.")]
    public int PublishedYear { get; set; }
}
```

#### Services/BookRepository.cs
```csharp
using LibraryApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace LibraryApi.Services;

public class BookRepository
{
    private readonly List<Book> _books = new();
    private int _nextId = 1;

    public BookRepository()
    {
        Add(new Book
        {
            Title = "1984",
            Author = "Джордж Орвелл",
            Genre = "Антиутопія",
            PublishedYear = 1949,
            PurchasePrice = 150.00m,
            InternalNotes = "Супер популярна книга, замовляти більше привезень."
        });

        Add(new Book
        {
            Title = "Великий Гетсбі",
            Author = "Ф. Скотт Фіцджеральд",
            Genre = "Класика",
            PublishedYear = 1925,
            PurchasePrice = 210.50m,
            InternalNotes = "Має пошкодження обкладинки на складі №2."
        });
    }

    public List<Book> GetAll()
    {
        return _books.ToList();
    }

    public Book? GetById(int id)
    {
        return _books.FirstOrDefault(b => b.Id == id);
    }

    public void Add(Book book)
    {

            book.Id = _nextId++;
            if (book.PurchasePrice == 0) book.PurchasePrice = 100.00m;
            _books.Add(book);

    }

    public bool Remove(int id)
    {

            var book = GetById(id);
            if (book is null) return false;
            _books.Remove(book);
            return true;

    }
}
```

#### Mapping/BookProfile.cs
```csharp
using AutoMapper;
using LibraryApi.Dtos;
using LibraryApi.Models;

namespace LibraryApi.Mapping;

public class BookProfile : Profile
{
    public BookProfile()
    {
        CreateMap<Book, BookDto>();
        CreateMap<CreateBookRequest, Book>();
    }
}
```

#### Controllers/BooksController.cs
```csharp
using AutoMapper;
using LibraryApi.Dtos;
using LibraryApi.Models;
using LibraryApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LibraryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class BooksController : ControllerBase
{
    private readonly BookRepository _repo;
    private readonly IMapper _mapper;

    public BooksController(BookRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<List<BookDto>> GetAll()
    {
        var books = _repo.GetAll();
        return Ok(_mapper.Map<List<BookDto>>(books));
    }

    [HttpGet("{id:int}")]
    public ActionResult<BookDto> GetById(int id)
    {
        var book = _repo.GetById(id);
        if (book is null) return NotFound();
        return Ok(_mapper.Map<BookDto>(book));
    }

    [HttpPost]
    [Consumes("application/json")]
    public ActionResult<BookDto> Create(CreateBookRequest request)
    {
        var book = _mapper.Map<Book>(request);
        _repo.Add(book);
        var dto = _mapper.Map<BookDto>(book);
        return CreatedAtAction(nameof(GetById), new { id = book.Id }, dto);
    }

    [HttpPut("{id:int}")]
    [Consumes("application/json")]
    public IActionResult Update(int id, CreateBookRequest request)
    {
        var book = _repo.GetById(id);
        if (book is null) return NotFound();
        _mapper.Map(request, book);
        return NoContent();
    }
}
```

#### Program.cs
```csharp
using LibraryApi.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi("v1", options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Title = "Library API";
        document.Info.Version = "v1";
        document.Info.Description = "API для керування каталогом книг.";
        return Task.CompletedTask;
    });
});

builder.Services.AddControllers();
builder.Services.AddSingleton<BookRepository>();
builder.Services.AddAutoMapper(cfg => { }, typeof(Program));

var app = builder.Build();

app.UseHttpsRedirection();
app.MapOpenApi();
app.MapScalarApiReference(options =>
{
    options.AddDocuments(["v1"]);
    options.WithTitle("Library API — документація");
});
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "Library API v1");
});
app.MapControllers();
app.Run();
```

---

## 2. Завдання для самостійної роботи

Ваше завдання — розширити функціонал проєкту `LibraryApi`, виконавши наступні вимоги в межах однієї спільної задачі:

### 1. Додавання видалення
* Додайте до `BooksController` HTTP-метод `DELETE api/books/{id}`. Якщо книгу знайдено і видалено, повертайте `204 NoContent`. Якщо книги з таким ID не існує — повертайте `404 NotFound`.

### 2. Фільтрація за автором
* Розширте метод `GetAll` у `BooksController` так, щоб він приймав опційний параметр `author` з query-рядка (наприклад, `GET api/books?author=Джордж`). 
* Якщо параметр передано, API має повертати лише ті книги, у яких ім'я або прізвище автора містить цей фрагмент тексту (пошук має бути нечутливим до регістру).

### 3. Робота з  Enums
* Створіть перерахування `BookGenre` з варіантами: `Fiction`, `Classics`, `Dystopia`, `Poetry`, `Fantasy` тощо.
* Замініть тип поля `Genre` у класі `Book` з `string` на `BookGenre`.
* Налаштуйте конфігурацію JSON-серіалізації в `Program.cs` так, щоб жанри у відповідях та запитах API передавалися у вигляді текстових назв (наприклад, `"Dystopia"`), а не у вигляді чисел (як-от `2`).

### 4. Челендж на AutoMapper: Вкладені сутності та згладжування
На практиці автори не зберігаються простим рядком. Необхідно реалізувати наступні зміни в моделях та налаштуваннях мапінгу:

1. **Створення вкладеної сутності**:
   * Створіть новий клас `Author` з властивостями: `FirstName` (рядок), `LastName` (рядок), `BirthYear` (ціле число).
2. **Оновлення моделей**:
   * У класі `Book` замініть властивість `string Author` на вкладений об'єкт `Author Author { get; set; }`.і
   * Відповідно оновіть початкові демо-дані в конструкторі `BookRepository`.
3. **Оновлення DTO запиту створення (`CreateBookRequest`)**:
   * Для спрощення передачі даних клієнтом, додайте до `CreateBookRequest` вкладений об'єкт `CreateAuthorRequest` (який містить `FirstName`, `LastName`, `BirthYear`). Клієнт при створенні книги має надсилати структуру автора всередині JSON запиту.
4. **Налаштування мапінгу в AutoMapper**:
   * Переналаштуйте `BookProfile` для обробки вкладеності.
   * **Згладжування (Flattening)**: У вихідному DTO `BookDto` властивість автора має лишатися плоским рядком `public string AuthorFullName { get; set; }`. За допомогою AutoMapper налаштуйте об'єднання імені та прізвища вкладеного об'єкта автора (наприклад, `"FirstName LastName"` -> `"Джордж Орвелл"`), щоб воно автоматично потрапляло в це поле.
   * **Обчислюване поле**: Додайте до `BookDto` властивість `public int AuthorAgeAtPublication { get; set; }`. За допомогою конфігурації `.ForMember(...)` та `.MapFrom(...)` в AutoMapper налаштуйте автоматичний розрахунок цього поля: воно має дорівнювати різниці між роком видання книги (`PublishedYear`) та роком народження її автора (`BirthYear`).

### Критерії перевірки:
* Проєкт успішно збирається командою `dotnet build`.
* Усі нові та оновлені API-методи валідно відображаються у Scalar OR Swagger UI.
* При виконанні GET-запитів чутливі поля `PurchasePrice` та `InternalNotes` не потрапляють у відповідь клієнту.
* Мапінг згладжування імені автора та обчислення віку автора на момент публікації книги працює повністю автоматично через налаштовані правила AutoMapper.