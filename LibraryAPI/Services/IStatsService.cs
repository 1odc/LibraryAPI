using LibraryAPI.Dtos;

namespace LibraryAPI.Services
{
    public interface IStatsService
    {
        Task<BookStatsDto> GetBookStatsAsync();
    }
}