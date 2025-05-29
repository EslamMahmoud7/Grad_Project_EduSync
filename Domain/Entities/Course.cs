using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Course
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Credits { get; set; }
        public string InstructorName { get; set; }
        public string InstructorEmail { get; set; }
        public string ResourceLink { get; set; } = string.Empty;
        public int Progress { get; set; }
        public DateTime NextDeadline { get; set; }
        public int Level { get; set; } = 0;
        public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
        public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
        public ICollection<Material> Materials { get; set; } = new List<Material>();
        public ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
        public ICollection<Lecture> Lectures { get; set; } = new List<Lecture>();
    }
}
