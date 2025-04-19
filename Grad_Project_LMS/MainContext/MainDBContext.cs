using Grad_Project_LMS.Models;
using Microsoft.EntityFrameworkCore;

namespace Grad_Project_LMS.MainContext
{
    public class MainDBContext : DbContext
    {
        public MainDBContext(DbContextOptions<MainDBContext> dbContext) : base(dbContext)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
        public DbSet<Student> students { get; set; }
    }
}
