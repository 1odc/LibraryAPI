using LibraryAPI.Models;

namespace LibraryAPI.Dtos
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string AuthorFullName { get; set; } = string.Empty;
        public BookGenre Genre { get; set; }
        public int PublishedYear { get; set; }
        public int AuthorAgeAtPublication { get; set; }
    }
}
