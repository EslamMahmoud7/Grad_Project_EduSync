using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class CreateInstructorDTO
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = default!;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = default!;

        [Required]
        [EmailAddress]
        [StringLength(256)]
        public string Email { get; set; } = default!;

        [Phone]
        [StringLength(20)]
        public string? PhoneNumber { get; set; }
    }

}
