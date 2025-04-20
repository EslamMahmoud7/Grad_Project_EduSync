using System.ComponentModel.DataAnnotations;

namespace Grad_Project_LMS.DTOs
{
    public class ResetPasswordDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Token { get; set; }
    }
}
