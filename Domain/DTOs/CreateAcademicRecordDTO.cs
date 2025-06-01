using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class CreateAcademicRecordDTO
    {
        [Required]
        public string StudentId { get; set; } = default!;
        [Required]
        public string GroupId { get; set; } = default!;
        public string? InstructorId { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Grade must be between 0 and 100.")]
        public int GradeValue { get; set; }
        [Required]
        public AssessmentType AssessmentType { get; set; }

        [Required]
        public string Term { get; set; } = default!;
        public AcademicRecordStatus Status { get; set; } = AcademicRecordStatus.Final;
    }
}
