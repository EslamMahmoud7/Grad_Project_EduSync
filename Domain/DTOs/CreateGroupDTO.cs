using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class CreateGroupDTO
    {
        [Required]
        public string CourseId { get; set; } = default!;
        [Required]
        public string Label { get; set; } = default!;
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }
        public string? Location { get; set; }
        public string? InstructorId { get; set; }
    }

}
