using LibraryAPI.Dtos;
using LibraryAPI.Models;

namespace LibraryAPI.Services
{
    public interface IBookService
    {
        Task<List<BookDto>> GetAllAsync();
        Task<BookDto?> GetByIdAsync(int id);
        Task<List<BookDto>> GetByGenreAsync(BookGenre genre);
        Task<BookDto> CreateAsync(CreateBookRequest request);
        Task<bool> UpdateAsync(int id, CreateBookRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
