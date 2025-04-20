using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
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
