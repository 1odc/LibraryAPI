namespace LibraryAPI.Models
{
    public enum BookGenre { Fiction, Classics, Dystopia, Poetry, Fantasy }
    public class Author
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int BirthYear { get; set; }
    }
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public Author Author { get; set; } = new Author();
        public BookGenre Genre { get; set; }
        public int PublishedYear { get; set; }

        // Чутливі дані, які мають бути приховані від читачів
        public decimal PurchasePrice { get; set; }
        public string InternalNotes { get; set; } = string.Empty;
    }
}
