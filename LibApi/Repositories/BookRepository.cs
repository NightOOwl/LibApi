using LibApi.Context;
using LibApi.Interfaces;
using LibApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LibApi.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly DataContext _context;

        public BookRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> BookExists(int id)
        {
            return await _context.Books.AnyAsync(b=>b.Id == id);
        }

        public async Task <bool> CreateBook(Book book)
        {
            _context.Add(book);
            return await Save();
        }

        public async Task <bool> DeleteBook(Book book)
        {
            _context.Remove(book);
            return await Save();
        }

        public async Task <Book> GetBook(int id)
        {
            return await _context.Books.Where(b => b.Id == id).FirstOrDefaultAsync();
        }

        public async Task<ICollection<Book>> GetBooks()
        {
            return await _context.Books.OrderBy(b => b.Id).ToListAsync();
        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task <bool> UpdateBook(Book book)
        {
              _context.Update(book);
            return await Save();
        }
    }
}
