using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class GradeSubmittedAssignmentDTO
    {
        [Required]
        [Range(0, 100)]
        public decimal Grade { get; set; }
        [StringLength(1000)]
        public string InstructorNotes { get; set; }
    }
}
