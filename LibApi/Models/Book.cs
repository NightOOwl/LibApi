namespace LibApi.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTimeOffset PublishDate { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
}
