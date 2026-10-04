using System.Runtime.InteropServices;
using AutoMapper;
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
        private readonly BooksRepository _repo;
        private readonly IMapper _mapper;

        public BooksController(BooksRepository BooksRepository, IMapper Mapper)
        {
            _repo = BooksRepository;
            _mapper = Mapper;
        }

        [HttpGet]
        [HttpGet]
        public ActionResult<List<BookDto>> GetAll([FromQuery] string? author)
        {
            var books = _repo.GetAll(author);
            return Ok(_mapper.Map<List<BookDto>>(books));
        }
        [HttpGet("{id:int}")]
        public ActionResult<BookDto> GetById(int id)
        {
            var book = _repo.GetById(id);
            if (book == null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<BookDto>(book);
            return Ok(dto);
        }
        [HttpPost("create")]
        public ActionResult<BookDto> Create(CreateBookRequest cbr)
        {
            var book = _mapper.Map<Book>(cbr);
            _repo.AddBook(book);
            var dto = _mapper.Map<BookDto>(book);
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            return _repo.Remove(id) ? NoContent() : NotFound();
        }
    }
}
