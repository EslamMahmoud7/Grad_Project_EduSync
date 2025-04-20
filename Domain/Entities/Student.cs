using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Student : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        [EmailAddress]
        public string? Address { get; set; }
        public string? NationalID { get; set; }
        public DateTime? EnrollmentDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime? FirstLogin { get; set; }

    }
}
