using System;

namespace Domain.DTOs
{
    public class UpdateGroupDTO
    {
        public string? Label { get; set; }
        public string? CourseId { get; set; }
        public string? InstructorId { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public string? Location { get; set; }
        public int? MaxStudents { get; set; }
    }
}
