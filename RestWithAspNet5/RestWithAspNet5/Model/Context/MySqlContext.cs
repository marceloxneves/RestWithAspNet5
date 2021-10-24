using Microsoft.EntityFrameworkCore;
using RestWithAspNet5.Authentication.Model;
using RestWithASPNETUdemy.Model;

namespace RestWithAspNet5.Model.Context
{
    public class MySqlContext : DbContext
    {
        public MySqlContext()
        { }

        public MySqlContext(DbContextOptions<MySqlContext> options) : base(options)
        { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
