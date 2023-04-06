using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LibApi.Dto
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTimeOffset PublishDate { get; set; }
        public int AuthorId { get; set; }
    }
}
