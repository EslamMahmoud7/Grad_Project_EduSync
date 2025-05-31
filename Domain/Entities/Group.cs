// Domain/Entities/Group.cs
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Group
    {
        public string Id { get; set; } = System.Guid.NewGuid().ToString();

        public string CourseId { get; set; } = default!;
        public Course Course { get; set; } = default!;
        public string? InstructorId { get; set; } = default!;
        public Instructor? Instructor { get; set; } = default!;

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Location { get; set; }
        public string? Label { get; set; }
        public ICollection<GroupStudent> GroupStudents { get; set; } = new List<GroupStudent>();
    }
}