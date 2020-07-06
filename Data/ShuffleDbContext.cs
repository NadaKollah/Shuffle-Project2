using Microsoft.EntityFrameworkCore;
using shuffle2.Entity;


namespace shuffle2.Data
{
    public class ShuffleDbContext : DbContext
    {
        public ShuffleDbContext(DbContextOptions<ShuffleDbContext> options) : base(options) { }

        public DbSet<User> user { get; set; }
       
    }

}

