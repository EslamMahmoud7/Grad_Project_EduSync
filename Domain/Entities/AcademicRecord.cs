// Domain/Entities/AcademicRecord.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class AcademicRecord
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string StudentId { get; set; } = default!;
        public User Student { get; set; } = default!;

        [Required]
        public string GroupId { get; set; } = default!;
        public Group Group { get; set; } = default!;

        public string? InstructorId { get; set; } 
        public Instructor? Instructor { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Grade must be between 0 and 100.")]
        public int GradeValue { get; set; }

        [Required]
        public AssessmentType AssessmentType { get; set; }

        [Required]
        [StringLength(50)]
        public string Term { get; set; } = default!;

        public DateTime DateRecorded { get; set; } = DateTime.UtcNow;
        public AcademicRecordStatus Status { get; set; } = AcademicRecordStatus.Final;
    }
}