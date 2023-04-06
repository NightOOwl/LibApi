﻿namespace LibApi.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}
