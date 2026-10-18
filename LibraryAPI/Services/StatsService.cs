using LibraryAPI.Data;
using LibraryAPI.Dtos;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Services
{
    public class StatsService : IStatsService
    {
        private readonly LibraryContext _context;

        public StatsService(LibraryContext context) => _context = context;

        public async Task<BookStatsDto> GetBookStatsAsync()
        {
            var total = await _context.Books.CountAsync();
            var avgYear = await _context.Books.AverageAsync(b => (double?)b.PublishedYear) ?? 0;

            return new BookStatsDto
            {
                TotalBooks = total,
                AveragePublishedYear = avgYear
            };
        }
    }
}