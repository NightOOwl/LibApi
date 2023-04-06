using LibApi.Context;
using LibApi.Interfaces;
using LibApi.Models;

namespace LibApi.Repositories
{
    public class AuthorRepository: IAuthorRepository
    {
        private readonly DataContext _context;

        public AuthorRepository(DataContext context)
        {
            _context = context;
        }

        public bool AuthorExists(int id)
        {
            return _context.Authors.Any(a => a.Id == id);
        }

        public bool CreateAuthor(Author author)
        {
            _context.Add(author);
             return Save();
        }

        public bool DeleteAuthor(Author author)
        {
            _context.Remove(author);
            return Save();
        }

        public Author GetAuthorById(int id)
        {
            return _context.Authors.Where(a => a.Id == id).FirstOrDefault();
        }

        public Author GetAuthorByLastName(string name)
        {
            return _context.Authors.Where(a => a.LastName == name).FirstOrDefault();
        }

        public ICollection<Author> GetAuthors()
        {
            return _context.Authors.OrderBy(a => a.Id).ToList();
        }

        public ICollection<Book> GetBooksByAuthorId(int id)
        {
            var author = GetAuthorById(id);
            return _context.Books.Where(a => a.AuthorId == author.Id).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateAuthor(Author author)
        {
            _context.Update(author);
            return Save();
        }
    }
}
