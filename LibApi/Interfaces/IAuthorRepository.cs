using LibApi.Models;

namespace LibApi.Interfaces
{
    public interface IAuthorRepository
    {
        Task<bool> AuthorExists(int id);
        Task <ICollection<Author>> GetAuthors();
        Task<Author> GetAuthorById(int id);
        Task<Author> GetAuthorByLastName(string name);
        Task<ICollection<Book>> GetBooksByAuthorId(int authorId);
        Task<bool> CreateAuthor(Author author);
        Task<bool> UpdateAuthor(Author author);
        Task<bool> DeleteAuthor(Author author);   
        Task<bool> Save();
    }
}
