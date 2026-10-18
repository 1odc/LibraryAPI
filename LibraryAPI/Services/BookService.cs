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

        public async Task<List<BookDto>> GetAllAsync()
        {
            return await _context.Books
                .AsNoTracking()
                .ProjectTo<BookDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
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