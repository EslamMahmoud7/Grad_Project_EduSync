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

    public async Task<CourseScheduleDTO> AddAsync(CreateCourseScheduleDTO dto)
    {
        var sched = new CourseSchedule
        {
            CourseId = dto.CourseId,
            Date = dto.Date,
            Time = dto.Time,
            Room = dto.Room,
            DoctorEmail = dto.DoctorEmail
        };

        _db.CourseSchedules.Add(sched);
        await _db.SaveChangesAsync();

        await _db.Entry(sched).Reference(s => s.Course).LoadAsync();

        return new CourseScheduleDTO
        {
            Date = sched.Date,
            Day = sched.Date.DayOfWeek.ToString(),
            Time = sched.Time,
            Subject = sched.Course.Title,
            Room = sched.Room,
            Doctor = sched.DoctorEmail
        };
    }

    public async Task<IReadOnlyList<CourseScheduleDTO>> GetForStudentAsync(string studentId)
    {
        var list = await _db.CourseSchedules
            .Include(cs => cs.Course)
            .Where(cs => cs.Course.StudentCourses
                .Any(sc => sc.StudentId == studentId))
            .ToListAsync();

        return list.Select(cs => new CourseScheduleDTO
        {
            Date = cs.Date,
            Day = cs.Date.DayOfWeek.ToString(),
            Time = cs.Time,
            Subject = cs.Course.Title,
            Room = cs.Room,
            Doctor = cs.DoctorEmail
        }).ToList();
    }
}
