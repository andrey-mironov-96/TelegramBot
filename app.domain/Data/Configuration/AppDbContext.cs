using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using app.domain.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace app.domain.Data.Configuration
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Faculty> Faculties { get; set; }

        
    }
}