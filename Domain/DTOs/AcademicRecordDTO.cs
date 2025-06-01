using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class AcademicRecordDTO
    {
        public string Id { get; set; } = default!;
        public string StudentId { get; set; } = default!;
        public string StudentFullName { get; set; } = default!;
        public string GroupId { get; set; } = default!;
        public string GroupLabel { get; set; } = default!;
        public string CourseId { get; set; } = default!;
        public string CourseCode { get; set; } = default!;
        public string CourseTitle { get; set; } = default!;
        public string Term { get; set; } = default!;
        public string? InstructorId { get; set; }
        public string? InstructorFullName { get; set; }
        public DateTime DateRecorded { get; set; }
        public AcademicRecordStatus Status { get; set; }
        public int GradeValue { get; set; }
        public AssessmentType AssessmentType { get; set; }
    }

}
