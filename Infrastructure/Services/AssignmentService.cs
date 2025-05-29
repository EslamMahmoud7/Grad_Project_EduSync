using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.IServices;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class AssignmentService : IAssignmentService
{
    private readonly MainDbContext _db;

    public AssignmentService(MainDbContext db)
    {
        _db = db;
    }

    public async Task<AssignmentDTO> AddAssignmentAsync(CreateAssignmentDTO dto)
    {
        var assignment = new Assignment
        {
            Title = dto.Title,
            Description = dto.Description,
            DueDate = dto.DueDate,
            CourseId = dto.CourseId
        };

        _db.Assignments.Add(assignment);
        await _db.SaveChangesAsync();

        return new AssignmentDTO
        {
            Title = assignment.Title,
            Description = assignment.Description,
            DueDate = assignment.DueDate
        };
    }

    public async Task<IReadOnlyList<AssignmentDTO>> GetForStudentAsync(string studentId)
    {
        var list = await _db.Assignments
            .Include(a => a.Course)
                .ThenInclude(c => c.StudentCourses)
            .Where(a => a.Course.StudentCourses.Any(sc => sc.StudentId == studentId))
            .ToListAsync();

        return list.Select(a => new AssignmentDTO
        {
            Title = a.Title,
            Description = a.Description,
            DueDate = a.DueDate,
            CourseTitle = a.Course.Title
        }).ToList();
    }
}
