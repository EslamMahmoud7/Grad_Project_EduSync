using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Grad_Project_LMS.Controller
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Token { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public UserRole Role { get; set; }
    }
}