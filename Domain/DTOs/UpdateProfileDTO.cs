using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class UpdateProfileDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Institution { get; set; }
        public string? Status { get; set; }
        public string? AvatarUrl { get; set; }
    }
}
