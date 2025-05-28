using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public UserRole Role { get; set; }
        public StudentProfile StudentProfile { get; set; }
        public AdminProfile AdminProfile { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}
