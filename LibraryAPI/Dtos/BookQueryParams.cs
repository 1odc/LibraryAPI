using System.ComponentModel.DataAnnotations;
using LibraryAPI.Models;

namespace LibraryAPI.Dtos
{
    public class BookQueryParams
    {
        /// <summary>Номер сторінки, починаючи з 1.</summary>
        [Range(1, int.MaxValue, ErrorMessage = "Page має бути більшим за 0.")]
        public int Page { get; set; } = 1;

        /// <summary>Кількість елементів на сторінці (1–50, за замовчуванням 5).</summary>
        [Range(1, 50, ErrorMessage = "Size має бути від 1 до 50.")]
        public int Size { get; set; } = 5;

        /// <summary>Фільтр за жанром.</summary>
        public BookGenre? Genre { get; set; }

        /// <summary>Фрагмент імені або прізвища автора.</summary>
        public string? Author { get; set; }

        /// <summary>Пошук у назві книги або імені автора.</summary>
        public string? Search { get; set; }

        /// <summary>Мінімальний рік видання.</summary>
        public int? MinYear { get; set; }

        /// <summary>Максимальний рік видання.</summary>
        public int? MaxYear { get; set; }

        /// <summary>Поле сортування: title, year, author, date.</summary>
        [RegularExpression("(?i)^(title|year|author|date)$", ErrorMessage = "SortBy: title, year, author або date.")]
        public string? SortBy { get; set; }

        /// <summary>Напрямок: asc або desc.</summary>
        [RegularExpression("(?i)^(asc|desc)$", ErrorMessage = "SortOrder: asc або desc.")]
        public string SortOrder { get; set; } = "asc";
    }
}