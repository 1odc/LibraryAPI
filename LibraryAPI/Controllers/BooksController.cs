using LibraryAPI.Dtos;
using LibraryAPI.Models;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Produces("application/json")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<BookDto>>> GetAll([FromQuery] BookQueryParams queryParams)
        {
            return Ok(await _bookService.GetAllAsync(queryParams));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BookDto>> GetById(int id)
        {
            var book = await _bookService.GetByIdAsync(id);
            return book is null ? NotFound() : Ok(book);
        }

        [HttpGet("by-genre")]
        public async Task<ActionResult<List<BookDto>>> GetByGenre([FromQuery] BookGenre genre)
        {
            return Ok(await _bookService.GetByGenreAsync(genre));
        }

        [HttpPost]
        public async Task<ActionResult<BookDto>> Create(CreateBookRequest request)
        {
            var dto = await _bookService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, CreateBookRequest request)
        {
            return await _bookService.UpdateAsync(id, request) ? NoContent() : NotFound();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await _bookService.DeleteAsync(id) ? NoContent() : NotFound();
        }
    }
}