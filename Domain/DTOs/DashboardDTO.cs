using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class DashboardDTO
    {
        public string FullName { get; set; } = default!;
        public double GPA { get; set; }
        public int TotalCourses { get; set; }
        public int PendingAssignments { get; set; }
        public List<LecatureDTO> TodaysClasses { get; set; } = new();
        public List<AssignmentDTO> UpcomingAssignments { get; set; } = new();
        public List<AnnouncementDTO> Announcements { get; set; } = new();
    }
}
