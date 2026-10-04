using System.ComponentModel.DataAnnotations;
using LibraryAPI.Models;

namespace LibraryAPI.Dtos
{
    public class CreateBookRequest
    {
        [Required(ErrorMessage = "Назва книги є обов'язковою.")]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Автор є обов'язковим.")]
        public CreateAuthorRequest Author { get; set; } = new();

        public BookGenre Genre { get; set; }

        [Range(1000, 2100)]
        public int PublishedYear { get; set; }
    }

    public class CreateAuthorRequest
    {
        [Required, MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Range(1, 2100)]
        public int BirthYear { get; set; }
    }
}
