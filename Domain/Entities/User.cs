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
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public UserRole Role { get; set; }
        public DateTime JoinedDate { get; set; } = DateTime.UtcNow;
        public string? Institution { get; set; }
        public int TotalCourses { get; set; }
        public double GPA { get; set; }
        public string? Status { get; set; }
        public string? AvatarUrl { get; set; }
        public ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
        public ICollection<GroupStudent> GroupStudents { get; set; } = new List<GroupStudent>();

    }

}
