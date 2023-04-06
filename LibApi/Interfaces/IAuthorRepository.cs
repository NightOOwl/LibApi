using LibApi.Models;

namespace LibApi.Interfaces
{
    public interface IAuthorRepository
    {
        bool AuthorExists(int id);
        ICollection<Author> GetAuthors();
        Author GetAuthorById(int id);
        Author GetAuthorByLastName(string name);
        ICollection<Book> GetBooksByAuthorId(int authorId);
        bool CreateAuthor(Author author);
        bool UpdateAuthor(Author author);
        bool DeleteAuthor(Author author);   
        bool Save();
    }
}
