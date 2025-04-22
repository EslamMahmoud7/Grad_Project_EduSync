using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace Infrastructure.Data
{
    public class IdentityContext : IdentityDbContext<Student>
    {
        public IdentityContext(DbContextOptions<IdentityContext> dbContext) : base(dbContext)
        {
            
        }
      
    }
}
