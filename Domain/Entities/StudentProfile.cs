using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class StudentProfile
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }

        public string Institution { get; set; }
        public double GPA { get; set; }
        public string Status { get; set; }
        public int TotalCourses { get; set; }
        public DateTime JoinedDate { get; set; }
        public string AvatarUrl { get; set; }

        public ICollection<Assignment> Assignments { get; set; }
        public ICollection<Quiz> Quizzes { get; set; }
        public ICollection<AcademicRecord> AcademicRecords { get; set; }
        public ICollection<ScheduleItem> Schedule { get; set; }
    }

}
