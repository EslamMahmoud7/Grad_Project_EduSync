using System;

namespace Domain.DTOs
{
    public class CourseScheduleDTO
    {
        public required string GroupId { get; set; }
        public DateTime Date { get; set; }
        public string Day { get; set; }
        public string Time { get; set; }
        public string Subject { get; set; }
        public string Room { get; set; }
        public string Doctor { get; set; }
    }
}
