using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class SubmittedAssignmentDTO
    {
        public string Id { get; set; }
        public string AssignmentId { get; set; }
        public string AssignmentTitle { get; set; }
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public string SubmissionTitle { get; set; }
        public string SubmissionLink { get; set; }
        public DateTime SubmissionDate { get; set; }
        public decimal? Grade { get; set; }
        public string InstructorNotes { get; set; }
    }
}
