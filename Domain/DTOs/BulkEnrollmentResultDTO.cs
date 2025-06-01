using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class BulkEnrollmentResultDTO
    {
        public string GroupId { get; set; } = default!;
        public int TotalStudentsProcessed { get; set; }
        public int StudentsEnrolledSuccessfully { get; set; }
        public List<string> FailedStudentIds { get; set; } = new List<string>();
        public Dictionary<string, string> Errors { get; set; } = new Dictionary<string, string>();
    }
}
