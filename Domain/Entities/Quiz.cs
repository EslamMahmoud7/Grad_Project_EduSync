using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Quiz
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime DueDate { get; set; }
        public QuizStatus Status { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public int? Score { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public int StudentProfileId { get; set; }
        public StudentProfile StudentProfile { get; set; }

        public string CreatedByAdminId { get; set; }
        public User CreatedByAdmin { get; set; }
    }
}
