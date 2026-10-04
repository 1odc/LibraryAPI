using LibraryAPI.Models;
namespace LibraryAPI.Services
{
    public class BooksRepository
    {
        private readonly List<Book> _books = new();
        private static int _booksId = 1;
        public BooksRepository()
        {
            AddBook(new Book
            {
                Title = "Великий Гетсбі",
                Author = new Author { FirstName = "Френсіс", LastName = "Скотт Фіцджеральд", BirthYear = 1896 },
                Genre = BookGenre.Classics,
                PublishedYear = 1925,
                PurchasePrice = 210.50m,
                InternalNotes = "Має пошкодження обкладинки на складі №2."
            });
            AddBook(new Book
            {
                Title = "Кобзар",
                Author = new Author { FirstName = "Тарас", LastName = "Шевченко", BirthYear = 1814 },
                Genre = BookGenre.Poetry,
                PublishedYear = 1840,
                PurchasePrice = 150.00m,
                InternalNotes = "Перевидання, стан гарний."
            });
            AddBook(new Book
            {
                Title = "1984",
                Author = new Author { FirstName = "Джордж", LastName = "Орвелл", BirthYear = 1903 },
                Genre = BookGenre.Dystopia,
                PublishedYear = 1949,
                PurchasePrice = 185.75m,
                InternalNotes = "Є легкі потертості на обкладинці."
            });
        }

        public IReadOnlyCollection<Book> GetAll(string? author = null)
        {
            IEnumerable<Book> result = _books;

            if (!string.IsNullOrWhiteSpace(author))
            {
                result = result.Where(b =>
                    b.Author.FirstName.Contains(author, StringComparison.OrdinalIgnoreCase) ||
                    b.Author.LastName.Contains(author, StringComparison.OrdinalIgnoreCase));
            }
            return result.ToList();
        }

        public Book? GetById(int id) => _books.FirstOrDefault(b => b.Id == id);
        public void AddBook(Book book)
        {
            book.Id = _booksId++;
            if (book.PurchasePrice == 0)
            {
                book.PurchasePrice = 100.00m;
            }
            
            _books.Add(book);
        }
        public bool Remove(int id)
        {
            var book = GetById(id);              
            return book != null && _books.Remove(book);
        }
    }
}
