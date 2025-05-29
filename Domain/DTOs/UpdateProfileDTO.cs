using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class UpdateProfileDTO
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string? PhoneNumber { get; set; }
        public string? Institution { get; set; }
        public string? AvatarUrl { get; set; }
        public int? TotalCourses { get; set; }
        public double? GPA { get; set; }
        public string? Status { get; set; }
    }
}
