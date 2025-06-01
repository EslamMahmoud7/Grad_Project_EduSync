// Infrastructure/Services/CourseScheduleService.cs
using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces.IServices;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class CourseScheduleService : ICourseScheduleService
{
    private readonly MainDbContext _db;

    public CourseScheduleService(MainDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<CourseScheduleDTO>> GetForStudentAsync(string studentId)
    {
        var studentGroups = await _db.GroupStudents
            .Where(gs => gs.StudentId == studentId)
            .Include(gs => gs.Group)
                .ThenInclude(g => g.Course)
            .Include(gs => gs.Group.Instructor)
            .Select(gs => gs.Group)
            .AsNoTracking()
            .ToListAsync();

        var scheduledLectures = studentGroups
            .Select(g =>
            {
                var doctorDisplayName = g.Instructor != null
                    ? $"{g.Instructor.FirstName} {g.Instructor.LastName}"
                    : "N/A";

                return new CourseScheduleDTO
                {
                    Date = g.StartTime.Date,
                    Day = g.StartTime.DayOfWeek.ToString(),
                    Time = g.StartTime.ToString("h:mm tt"),
                    Subject = g.Course.Title,
                    Room = g.Location ?? "N/A",
                    Doctor = doctorDisplayName
                };
            })
            .OrderBy(dto => dto.Date)
            .ToList();

        return scheduledLectures;
    }
}