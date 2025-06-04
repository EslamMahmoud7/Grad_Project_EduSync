// Domain/Entities/SubmittedAssignment.cs
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class SubmittedAssignment
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; }
        public string SubmissionLink { get; set; }
        public DateTime SubmissionDate { get; set; } = DateTime.UtcNow;
        public decimal? Grade { get; set; }
        public string InstructorNotes { get; set; } = string.Empty;

        public string AssignmentId { get; set; }
        public string StudentId { get; set; }

        [ForeignKey("AssignmentId")]
        public Assignment Assignment { get; set; }
        [ForeignKey("StudentId")]
        public User Student { get; set; }
    }
}