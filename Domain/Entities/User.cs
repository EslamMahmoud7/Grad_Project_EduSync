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
        public DateTime JoinedDate { get; set; } = default!;
        public string Institution { get; set; } = default!;
        public int TotalCourses { get; set; }
        public double GPA { get; set; }
        public string Status { get; set; } = default!;
        public string AvatarUrl { get; set; } = default!;
        public ICollection<GroupStudent> GroupStudents { get; set; } = new List<GroupStudent>();
        public IList<string> Achievements { get; set; } = new List<string>();
        public IList<string> RecentActivity { get; set; } = new List<string>();
        public IList<string> SocialLinks { get; set; } = new List<string>();

    }

}
