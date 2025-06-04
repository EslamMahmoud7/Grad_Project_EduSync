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
                    GroupId = g.Id,
                    Date = g.StartTime, 
                    Day = g.StartTime.ToString("dddd"),
                    Time = g.StartTime.ToString("hh:mm tt"),
                    Subject = g.Course.Title,
                    Room = g.Location ?? "N/A",
                    Doctor = doctorDisplayName
                };
            })
            .OrderBy(dto => dto.Date)
            .ToList();

        return scheduledLectures;
    }

    public async Task<IReadOnlyList<CourseScheduleDTO>> GetForInstructorAsync(string instructorId)
    {
        var instructorGroups = await _db.Groups
            .Where(g => g.InstructorId == instructorId)
            .Include(g => g.Course)
            .Include(g => g.Instructor)
            .AsNoTracking()
            .ToListAsync();

        var scheduledLectures = instructorGroups
            .Select(g =>
            {
                var doctorDisplayName = g.Instructor != null
                    ? $"{g.Instructor.FirstName} {g.Instructor.LastName}"
                    : "N/A";

                return new CourseScheduleDTO
                {
                    GroupId = g.Id,
                    Date = g.StartTime,
                    Day = g.StartTime.ToString("dddd"),
                    Time = g.StartTime.ToString("hh:mm tt"),
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