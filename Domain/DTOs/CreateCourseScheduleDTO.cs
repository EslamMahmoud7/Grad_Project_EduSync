using System;

namespace Domain.DTOs
{
    public class CreateCourseScheduleDTO
    {
        public string CourseId { get; set; } = default!;

        public DateTime Date { get; set; }

        public string Time { get; set; } = default!;

        public string Room { get; set; } = default!;
        public string DoctorEmail { get; set; } = default!;
    }
}
