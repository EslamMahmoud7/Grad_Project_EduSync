using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Group
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string CourseId { get; set; } = default!;
        public Course Course { get; set; } = default!;

        public string? InstructorId { get; set; }
        public User? Instructor { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Location { get; set; }
        public string? Label { get; set; }

        public ICollection<GroupStudent> GroupStudents { get; set; } = new List<GroupStudent>();
        public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
        public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
        public ICollection<Material> Materials { get; set; } = new List<Material>();
    }
}