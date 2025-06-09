using Domain.DTOs;
using Domain.Interfaces.IServices;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

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

            var totalCourses = await _db.GroupStudents
                .Where(gs => gs.StudentId == studentId)
                .Select(gs => gs.Group.CourseId)
                .Distinct()
                .CountAsync();

            var pendingAssignments = await _db.Assignments
                .Where(a =>
                    a.DueDate > DateTime.UtcNow
                    && a.Group.GroupStudents.Any(gs => gs.StudentId == studentId)
                )
                .CountAsync();

            var today = DateTime.UtcNow.Date;
            var todaysClasses = await _db.Groups
                .Where(g =>
                    g.StartTime.Date == today
                    && g.GroupStudents.Any(gs => gs.StudentId == studentId)
                )
                .Include(g => g.Course)
                .Include(g => g.Instructor)
                .ToListAsync();

            var lectureDtos = todaysClasses
                .Select(g =>
                {
                    var doctorDisplayName = g.Instructor != null
                        ? $"{g.Instructor.FirstName} {g.Instructor.LastName}"
                        : "N/A";

                    return new LectureDTO
                    {
                        Time = g.StartTime.ToString("h:mm tt"),
                        Subject = g.Course.Title,
                        Doctor = doctorDisplayName
                    };
                })
                .ToList();

            var upcomingAssignments = await _db.Assignments
                .Where(a =>
                    a.DueDate > DateTime.UtcNow
                    && a.Group.GroupStudents.Any(gs => gs.StudentId == studentId)
                )
                .OrderBy(a => a.DueDate)
                .Take(5)
                .Select(a => new AssignmentDTO
                {
                    Title = a.Title,
                    DueDate = a.DueDate,
                })
                .ToListAsync();

            var announcements = await _db.Announcements
                .OrderByDescending(n => n.Date)
                .Take(5)
                .Select(n => new AnnouncementDTO
                {
                    Title = n.Title,
                    Date = n.Date,
                    Message = n.Message
                })
                .ToListAsync();

            return new DashboardDTO
            {
                FullName = $"{user.FirstName} {user.LastName}",
                GPA = user.GPA,
                TotalCourses = totalCourses,
                PendingAssignments = pendingAssignments,
                TodaysClasses = lectureDtos,
                UpcomingAssignments = upcomingAssignments,
                Announcements = announcements,
            };
        }
    }
}