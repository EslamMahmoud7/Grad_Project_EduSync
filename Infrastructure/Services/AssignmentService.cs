using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.IServices;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class AssignmentService : IAssignmentService
{
    private readonly MainDbContext _db;
    private readonly IGenericRepository<Assignment> _assignmentRepo;

    public AssignmentService(MainDbContext db, IGenericRepository<Assignment> assignmentRepo)
    {
        _db = db;
        _assignmentRepo = assignmentRepo;
    }

    public async Task<AssignmentDTO> AddAssignmentAsync(CreateAssignmentDTO dto)
    {
        var group = await _db.Groups.FindAsync(dto.GroupId);
        if (group == null)
        {
            throw new ArgumentException($"Group with ID '{dto.GroupId}' not found.");
        }

        var assignment = new Assignment
        {
            Id = Guid.NewGuid().ToString(),
            Title = dto.Title,
            Description = dto.Description,
            DueDate = dto.DueDate,
            GroupId = dto.GroupId
        };

        await _assignmentRepo.Add(assignment);

        var addedAssignment = await _db.Assignments
            .Where(a => a.Id == assignment.Id)
            .Include(a => a.Group)
                .ThenInclude(g => g.Course)
            .FirstOrDefaultAsync();

        return MapToDto(addedAssignment);
    }

    public async Task<IReadOnlyList<StudentAssignmentDTO>> GetForStudentAsync(string studentId)
    {
        var assignments = await _db.Assignments
            .Include(a => a.Group)
                .ThenInclude(g => g.Course)
            .Where(a => a.Group.GroupStudents.Any(gs => gs.StudentId == studentId))
            .AsNoTracking()
            .ToListAsync();

        var studentSubmissions = await _db.SubmittedAssignments
            .Where(sa => sa.StudentId == studentId)
            .ToDictionaryAsync(sa => sa.AssignmentId, sa => sa);

        var studentAssignmentDtos = assignments.Select(assignment =>
        {
            var currentStatus = AssignmentStatus.Pending;
            SubmittedAssignment submission = null;

            if (studentSubmissions.TryGetValue(assignment.Id, out submission))
            {
                currentStatus = submission.Grade.HasValue
                    ? AssignmentStatus.Submitted
                    : AssignmentStatus.Graded;
            }

            return new StudentAssignmentDTO
            {
                Id = assignment.Id,
                Title = assignment.Title,
                Description = assignment.Description,
                DueDate = assignment.DueDate,
                CourseTitle = assignment.Group?.Course?.Title ?? "N/A",
                GroupLabel = assignment.Group?.Label ?? "N/A",

                SubmissionStatus = currentStatus.ToString(),

                SubmissionDate = submission?.SubmissionDate,
                Grade = submission?.Grade,
                InstructorNotes = submission?.InstructorNotes,
                SubmissionLink = submission?.SubmissionLink,
                SubmissionId = submission?.Id
            };
        }).ToList();

        return studentAssignmentDtos;
    }

    public async Task<AssignmentDTO> GetAssignmentByIdAsync(string id)
    {
        var assignment = await _db.Assignments
            .Include(a => a.Group)
                .ThenInclude(g => g.Course)
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id);

        if (assignment == null) throw new ArgumentException($"Assignment with ID '{id}' not found.");
        return MapToDto(assignment);
    }

    public async Task<IReadOnlyList<AssignmentDTO>> GetAllAssignmentsAsync()
    {
        var assignments = await _db.Assignments
            .Include(a => a.Group)
                .ThenInclude(g => g.Course)
            .AsNoTracking()
            .ToListAsync();
        return assignments.Select(MapToDto).ToList();
    }

    public async Task<AssignmentDTO> UpdateAssignmentAsync(string id, UpdateAssignmentDTO dto)
    {
        var assignment = await _db.Assignments.FirstOrDefaultAsync(a => a.Id == id);
        if (assignment == null) throw new ArgumentException($"Assignment with ID '{id}' not found.");

        if (dto.Title != null) assignment.Title = dto.Title;
        if (dto.Description != null) assignment.Description = dto.Description;
        if (dto.DueDate.HasValue) assignment.DueDate = dto.DueDate.Value;
        if (dto.Status.HasValue) assignment.Status = dto.Status.Value;

        await _assignmentRepo.Update(assignment);
        return await GetAssignmentByIdAsync(id);
    }

    public async Task DeleteAssignmentAsync(string id)
    {
        var assignment = await _db.Assignments.FirstOrDefaultAsync(a => a.Id == id);
        if (assignment == null) throw new ArgumentException($"Assignment with ID '{id}' not found.");
        await _assignmentRepo.Delete(assignment);
    }


    private AssignmentDTO MapToDto(Assignment assignment)
    {
        return new AssignmentDTO
        {
            Id = assignment.Id,
            Title = assignment.Title,
            Description = assignment.Description,
            DueDate = assignment.DueDate,
            Status = assignment.Status,
            GroupId = assignment.GroupId,
            GroupLabel = assignment.Group?.Label ?? "N/A",
            CourseTitle = assignment.Group?.Course?.Title ?? "N/A"
        };
    }
}