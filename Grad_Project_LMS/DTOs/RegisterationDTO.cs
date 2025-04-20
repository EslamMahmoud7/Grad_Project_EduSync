using System.ComponentModel.DataAnnotations;

namespace Grad_Project_LMS.DTOs
{
    public class RegisterationDTO
    {
        [EmailAddress]
        [Required]

        public string email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        [Phone]
        public string phonenumber { get; set; }
    }
}
