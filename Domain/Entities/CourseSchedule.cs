using System;

namespace Domain.Entities
{
    public class CourseSchedule
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string CourseId { get; set; } = default!;
        public Course Course { get; set; } = default!;

        public DateTime Date { get; set; }

        public string Time { get; set; }

        public string Room { get; set; } = default!;
        public string DoctorEmail { get; set; } = default!;
    }
}
