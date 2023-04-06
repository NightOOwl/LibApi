using LibApi.Context;
using LibApi.Interfaces;
using LibApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LibApi.Repositories
{
    public class AuthorRepository: IAuthorRepository
    {
        private readonly DataContext _context;

        public AuthorRepository(DataContext context)
        {
            _context = context;
        }

        public async Task <bool> AuthorExists(int id)
        {
            return  await  _context.Authors.AnyAsync(a => a.Id == id);
        }

        public async Task<bool> CreateAuthor(Author author)
        {
            _context.Add(author);
             return await Save();
        }

        public async Task<bool> DeleteAuthor(Author author)
        {
            _context.Remove(author);
            return await Save();
        }

        public async Task<Author> GetAuthorById(int id)
        {
            return await _context.Authors.Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Author> GetAuthorByLastName(string name)
        {
            return await _context.Authors.Where(a => a.LastName == name).FirstOrDefaultAsync();
        }

        public async Task <ICollection<Author>> GetAuthors()
        {
            return await _context.Authors.OrderBy(a => a.Id).ToListAsync();
        }

        public async Task<ICollection<Book>> GetBooksByAuthorId(int id)
        {
            var author = GetAuthorById(id);
            return await _context.Books.Where(a => a.AuthorId == author.Id).ToListAsync();
        }

        public async Task <bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task <bool> UpdateAuthor(Author author)
        {
            _context.Update(author);
            return await Save();
        }
    }
}
