using app.domain.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace app.domain.Data.Configuration
{
    public class AppDbContext : DbContext
    {
        #pragma warning disable CS8618
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Specialty> Specialies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FacultyConfiguration());
            modelBuilder.ApplyConfiguration(new SpecialtyConfiguration());
        }

    }
}