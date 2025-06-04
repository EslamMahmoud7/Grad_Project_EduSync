using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class SubmitAssignmentDTO
    {
        [Required]
        public string AssignmentId { get; set; }
        [Required]
        public string StudentId { get; set; }
        [Required]
        [StringLength(200)]
        public string Title { get; set; }
        [Required]
        [Url]
        [StringLength(1000)]
        public string SubmissionLink { get; set; }
    }
}
