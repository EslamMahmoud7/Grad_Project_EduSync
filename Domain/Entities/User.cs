using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string PhoneNumber { get; set; }
        public string Address { get; set; } = string.Empty;
        public required string NationalID { get; set; }
        public required string ProfileImage { get; set; } = string.Empty; 
        public DateTime EnrollmentDate { get; set; } = DateTime.Now;
        public DateTime LastLoginDate { get; set; } = DateTime.Now;
        public DateTime FirstLogin { get; set; } = DateTime.Now;
    }
}
