using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class CreateAssignmentDTO
    {
        [Required]
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
        [Required]
        public string GroupId { get; set; } = default!;
    }
}
