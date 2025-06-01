using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class UpdateAcademicRecordDTO
    {
        [Range(0, 100, ErrorMessage = "Grade must be between 0 and 100.")]
        public int? GradeValue { get; set; }
        public AssessmentType? AssessmentType { get; set; }

        public string? Term { get; set; }
        public string? InstructorId { get; set; }
        public AcademicRecordStatus? Status { get; set; }
        public DateTime? DateRecorded { get; set; }
    }
}
