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
    public class AuthorController : Controller
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;
        public AuthorController(IAuthorRepository authorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Author>))]
        public async Task <IActionResult> GetAuthors()
        {
            var authors = _mapper.Map<List<AuthorDto>>( await _authorRepository.GetAuthors());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(authors);
        }
        [HttpGet("{authorId}")]
        [ProducesResponseType(200, Type = typeof(Author))]
        [ProducesResponseType(400)]
        public async Task <IActionResult> GetAuthor(int authorId)
        {
            if (! await _authorRepository.AuthorExists(authorId))
                return NotFound();
            var author = _mapper.Map<AuthorDto>(_authorRepository.GetAuthorById(authorId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(author);
        }
        [HttpGet("book/{authorId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Book>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetBookByAuthorId(int authorId)
        {
            var books = _mapper.Map<List<BookDto>>
                ( await _authorRepository.GetBooksByAuthorId(authorId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(books);
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task <IActionResult> CreateAuthor([FromBody] AuthorDto authorCreate)
        {
            if (authorCreate== null)
            {
                return BadRequest(ModelState);
            }
            var authors = await _authorRepository.GetAuthors();
            var author=authors.Where(a => a.FirstName.Trim().ToUpper() == authorCreate.FirstName.ToUpper()
                && a.LastName.Trim().ToUpper() == authorCreate.LastName.ToUpper())
                .FirstOrDefault();
            if (author!=null)
            {
                ModelState.AddModelError("", "Author already exist");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
                BadRequest(ModelState); 
            var authorMap=_mapper.Map<Author>(authorCreate);
            if (! await _authorRepository.CreateAuthor(authorMap))
            {
                ModelState.AddModelError("","Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully created");
        }
        [HttpDelete("{authorId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteAuthor(int authorId)
        {
            if (! await _authorRepository.AuthorExists(authorId))
            {
                return NotFound();
            }
            var authorToDelete = await _authorRepository.GetAuthorById(authorId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (! await _authorRepository.DeleteAuthor(authorToDelete))
            {
                ModelState.AddModelError("", "Something went wrong while deleting the author");
            }
            return NoContent();
        }
        [HttpPut("{authorId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task <IActionResult> UpdateAuthor(int authorId, [FromBody] AuthorDto updatedAuthor)
        {
            if (updatedAuthor==null)
            {
                return BadRequest(ModelState);
            }
            if (authorId !=updatedAuthor.Id)
            {
                return BadRequest(ModelState);
            }
            if (! await _authorRepository.AuthorExists(authorId))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var authorMap=_mapper.Map<Author>(updatedAuthor);
            if (! await _authorRepository.UpdateAuthor(authorMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating author");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

    }
}
