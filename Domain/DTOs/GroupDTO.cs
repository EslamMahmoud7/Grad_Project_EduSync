using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class GroupDTO
    {
        public string Id { get; set; } = default!;
        public string CourseId { get; set; } = default!;
        public string CourseTitle { get; set; } = default!;
        public string? CourseDescription { get; set; }
        public int CourseCredits { get; set; }
        public string? CourseResourceLink { get; set; }
        public int CourseLevel { get; set; }
        public string Label { get; set; } = default!;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Location { get; set; }
        public InstructorDTO? Instructor { get; set; }
        public int NumberOfStudents { get; set; }
    }
}
