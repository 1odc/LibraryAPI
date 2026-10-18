using AutoMapper;
using AutoMapper.QueryableExtensions;
using LibraryAPI.Data;
using LibraryAPI.Dtos;
using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Services
{
    public class BookService : IBookService
    {
        private readonly LibraryContext _context;
        private readonly IMapper _mapper;

        public BookService(LibraryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedResult<BookDto>> GetAllAsync(BookQueryParams p)
        {
            IQueryable<Book> query = _context.Books.AsNoTracking();

            if (p.Genre.HasValue)
                query = query.Where(b => b.Genre == p.Genre.Value);

            if (!string.IsNullOrWhiteSpace(p.Author))
                query = query.Where(b => b.Author.FirstName.Contains(p.Author)
                                      || b.Author.LastName.Contains(p.Author));

            if (!string.IsNullOrWhiteSpace(p.Search))
                query = query.Where(b => b.Title.Contains(p.Search)
                                      || b.Author.FirstName.Contains(p.Search)
                                      || b.Author.LastName.Contains(p.Search));

            if (p.MinYear.HasValue)
                query = query.Where(b => b.PublishedYear >= p.MinYear.Value);

            if (p.MaxYear.HasValue)
                query = query.Where(b => b.PublishedYear <= p.MaxYear.Value);

            var totalItems = await query.CountAsync();

            var desc = string.Equals(p.SortOrder, "desc", StringComparison.OrdinalIgnoreCase);
            query = p.SortBy?.ToLowerInvariant() switch
            {
                "title" => desc ? query.OrderByDescending(b => b.Title).ThenByDescending(b => b.Id)
                                 : query.OrderBy(b => b.Title).ThenBy(b => b.Id),
                "year" => desc ? query.OrderByDescending(b => b.PublishedYear).ThenByDescending(b => b.Id)
                                 : query.OrderBy(b => b.PublishedYear).ThenBy(b => b.Id),
                "author" => desc ? query.OrderByDescending(b => b.Author.LastName)
                                        .ThenByDescending(b => b.Author.FirstName).ThenByDescending(b => b.Id)
                                 : query.OrderBy(b => b.Author.LastName)
                                        .ThenBy(b => b.Author.FirstName).ThenBy(b => b.Id),
                "date" => desc ? query.OrderByDescending(b => b.CreatedAt).ThenByDescending(b => b.Id)
                                 : query.OrderBy(b => b.CreatedAt).ThenBy(b => b.Id),
                _ => query.OrderBy(b => b.Id)
            };

            var items = await query
                .Skip((p.Page - 1) * p.Size)
                .Take(p.Size)
                .ProjectTo<BookDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new PagedResult<BookDto>(items, totalItems, p.Page, p.Size);
        }

        public async Task<BookDto?> GetByIdAsync(int id)
        {
            return await _context.Books
                .AsNoTracking()
                .Where(b => b.Id == id)
                .ProjectTo<BookDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<List<BookDto>> GetByGenreAsync(BookGenre genre)
        {
            return await _context.Books
                .AsNoTracking()
                .Where(b => b.Genre == genre)
                .ProjectTo<BookDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<BookDto> CreateAsync(CreateBookRequest request)
        {
            var book = _mapper.Map<Book>(request);
            book.CreatedAt = DateTime.UtcNow;
            book.StorageLocation = "Головний склад, Сектор А";
            book.PurchasePrice = 100.00m;

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return _mapper.Map<BookDto>(book);
        }

        public async Task<bool> UpdateAsync(int id, CreateBookRequest request)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (book is null) return false;

            _mapper.Map(request, book);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (book is null) return false;

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}