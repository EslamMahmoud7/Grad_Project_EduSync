using System.ComponentModel.DataAnnotations;

namespace Grad_Project_LMS.DTOs
{
    public class ForgetPasswordDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
