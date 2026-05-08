using Microsoft.EntityFrameworkCore;

namespace CloudDevProj2.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        { 
        }
        public DbSet<Person> People { get; set; }

        public DbSet<Skill> Skills { get; set; }
        public DbSet<Notes> Notes { get; set; }
    }
}
