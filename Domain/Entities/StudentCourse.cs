using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class StudentCourse
    {
        public string StudentId { get; set; } 
        public User Student { get; set; }
        public string CourseId { get; set; } 
        public Course Course { get; set; }
    }
}
