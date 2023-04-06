using LibApi.Models;

namespace LibApi.Interfaces
{
    public interface IBookRepository
    {
       Task<ICollection<Book>> GetBooks();
       Task<Book> GetBook(int id);
       Task <bool> BookExists(int id);
       Task <bool> CreateBook(Book book);
       Task <bool> UpdateBook(Book book);
       Task <bool> DeleteBook(Book book);
       Task <bool> Save();
    }
}
