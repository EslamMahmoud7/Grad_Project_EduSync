using Domain.DTOs;
using Domain.Interfaces.IServices;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly MainDbContext _db;
        public DashboardService(MainDbContext db) => _db = db;

        public async Task<DashboardDTO> GetForStudentAsync(string studentId)
        {
            var user = await _db.Users.FindAsync(studentId)
                       ?? throw new ArgumentException("User not found");

            var totalCourses = await _db.StudentCourses
                 .CountAsync(sc => sc.StudentId == studentId);

            var pendingAssignments = await _db.Assignments
                 .Where(a => a.Course.StudentCourses
                       .Any(sc => sc.StudentId == studentId)
                     && a.DueDate > DateTime.UtcNow)
                 .CountAsync();

            var today = DateTime.UtcNow.Date;
            var todaysClasses = await _db.Lectures
                .Include(l => l.Course)
                .Where(l => l.Date.Date == today
                         && l.Course.StudentCourses
                            .Any(sc => sc.StudentId == studentId))
                .Select(l => new LecatureDTO
                {
                    Time = l.Date.ToString("h:mm tt"),
                    Subject = l.Course.Title,
                    Doctor = l.InstructorName
                })
                .ToListAsync();

            var upcomingAssignments = await _db.Assignments
                .Where(a => a.Course.StudentCourses
                             .Any(sc => sc.StudentId == studentId)
                            && a.DueDate > DateTime.UtcNow)
                .OrderBy(a => a.DueDate)
                .Take(5)
                .Select(a => new AssignmentDTO
                {
                    Title = a.Title,
                    DueDate = a.DueDate
                })
                .ToListAsync();

            var announcements = await _db.Announcements
                .OrderByDescending(n => n.Date)
                .Take(5)
                .Select(n => new AnnouncementDTO
                {
                    Title = n.Title,
                    Date = n.Date
                })
                .ToListAsync();

            return new DashboardDTO
            {
                FullName = $"{user.FirstName} {user.LastName}",
                GPA = user.GPA,
                TotalCourses = totalCourses,
                PendingAssignments = pendingAssignments,
                TodaysClasses = todaysClasses,
                UpcomingAssignments = upcomingAssignments,
                Announcements = announcements
            };
        }
    }
}
