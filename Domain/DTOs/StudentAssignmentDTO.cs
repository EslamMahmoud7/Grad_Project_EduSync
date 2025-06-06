using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class StudentAssignmentDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public string CourseTitle { get; set; }
        public string GroupLabel { get; set; }

        public string SubmissionStatus { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public decimal? Grade { get; set; }
        public string InstructorNotes { get; set; }
        public string SubmissionLink { get; set; }
        public string SubmissionId { get; set; }
    }
}
