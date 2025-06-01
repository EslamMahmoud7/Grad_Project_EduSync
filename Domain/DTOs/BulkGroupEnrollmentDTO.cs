using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
     public class BulkGroupEnrollmentDTO
    {
        [Required]
        public string GroupId { get; set; } = default!;

        [Required]
        [MinLength(1, ErrorMessage = "At least one student ID is required for bulk enrollment.")]
        public List<string> StudentIds { get; set; } = new List<string>();
    }
}
