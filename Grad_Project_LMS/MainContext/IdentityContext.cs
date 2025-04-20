using Grad_Project_LMS.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Grad_Project_LMS.MainContext
{
    public class IdentityContext : IdentityDbContext<Student>
    {
        public IdentityContext(DbContextOptions<IdentityContext> dbContext) : base(dbContext)
        {
            
        }
    }
}
