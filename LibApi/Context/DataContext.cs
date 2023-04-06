using LibApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace LibApi.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
    }
}
