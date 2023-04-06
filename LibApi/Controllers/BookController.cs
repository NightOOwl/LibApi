using AutoMapper;
using LibApi.Dto;
using LibApi.Interfaces;
using LibApi.Models;
using LibApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LibApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public BookController(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Book>))]
        public async Task <IActionResult> GetBooks()
        {
            var books = _mapper.Map<List<BookDto>>( await _bookRepository.GetBooks());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(books);
        }
        [HttpGet("{bookId}")]
        [ProducesResponseType(200, Type = typeof(Book))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetBook(int bookId)
        {
            if ( ! await _bookRepository.BookExists(bookId))
                return NotFound();
            var book = _mapper.Map<BookDto>( await _bookRepository.GetBook(bookId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(book);
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateBook([FromBody] BookDto bookCreate)
        {
            if ( bookCreate == null)
            {
                return BadRequest(ModelState);
            }
            var books = await _bookRepository.GetBooks();
            var book= books.Where(a => a.Title.Trim().ToUpper() == bookCreate.Title.Trim().ToUpper())
                .FirstOrDefault();
            if (book != null)
            {
                ModelState.AddModelError("", "Book already exist");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
                BadRequest(ModelState);
            var bookMap = _mapper.Map<Book>(bookCreate);
            if (! await _bookRepository.CreateBook(bookMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully created");
        }
        [HttpDelete("{bookId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task <IActionResult> DeleteBook(int bookId)
        {
            if (! await _bookRepository.BookExists(bookId))
            {
                return NotFound();
            }
            var bookToDelete = await _bookRepository.GetBook(bookId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (! await  _bookRepository.DeleteBook(bookToDelete))
            {
                ModelState.AddModelError("", "Something went wrong while deleting the book");
            }
            return NoContent();
        }
        [HttpPut("{bookId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task <IActionResult> UpdateBook(int bookId, [FromBody] BookDto updatedBook)
        {
            if (updatedBook == null)
            {
                return BadRequest(ModelState);
            }
            if (bookId != updatedBook.Id)
            {
                return BadRequest(ModelState);
            }
            if (! await _bookRepository.BookExists(bookId))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var bookMap = _mapper.Map<Book>(updatedBook);
            if (! await _bookRepository.UpdateBook(bookMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating book");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
