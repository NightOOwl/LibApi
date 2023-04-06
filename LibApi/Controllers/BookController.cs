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
        public IActionResult GetBooks()
        {
            var books = _mapper.Map<List<BookDto>>(_bookRepository.GetBooks());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(books);
        }
        [HttpGet("{bookId}")]
        [ProducesResponseType(200, Type = typeof(Book))]
        [ProducesResponseType(400)]
        public IActionResult GetBook(int bookId)
        {
            if (!_bookRepository.BookExists(bookId))
                return NotFound();
            var book = _mapper.Map<BookDto>(_bookRepository.GetBook(bookId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(book);
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateBook([FromBody] BookDto bookCreate)
        {
            if (CreateBook == null)
            {
                return BadRequest(ModelState);
            }
            var book = _bookRepository.GetBooks()
                .Where(a => a.Title.Trim().ToUpper() == bookCreate.Title.ToUpper())
                .FirstOrDefault();
            if (book != null)
            {
                ModelState.AddModelError("", "Book already exist");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
                BadRequest(ModelState);
            var bookMap = _mapper.Map<Book>(bookCreate);
            if (!_bookRepository.CreateBook(bookMap))
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
        public IActionResult DeleteBook(int bookId)
        {
            if (!_bookRepository.BookExists(bookId))
            {
                return NotFound();
            }
            var bookToDelete = _bookRepository.GetBook(bookId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_bookRepository.DeleteBook(bookToDelete))
            {
                ModelState.AddModelError("", "Something went wrong while deleting the book");
            }
            return NoContent();
        }
        [HttpPut("{bookId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateBook(int bookId, [FromBody] BookDto updatedBook)
        {
            if (updatedBook == null)
            {
                return BadRequest(ModelState);
            }
            if (bookId != updatedBook.Id)
            {
                return BadRequest(ModelState);
            }
            if (!_bookRepository.BookExists(bookId))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var bookMap = _mapper.Map<Book>(updatedBook);
            if (!_bookRepository.UpdateBook(bookMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating book");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
