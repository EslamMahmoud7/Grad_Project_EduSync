using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.IServices;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class SubmittedAssignmentService : ISubmittedAssignmentService
{
    private readonly MainDbContext _db;
    private readonly IGenericRepository<SubmittedAssignment> _submittedAssignmentRepo;
    private readonly IGenericRepository<Assignment> _assignmentRepo;
    private readonly IGenericRepository<User> _studentRepo;

    public SubmittedAssignmentService(
        MainDbContext db,
        IGenericRepository<SubmittedAssignment> submittedAssignmentRepo,
        IGenericRepository<Assignment> assignmentRepo,
        IGenericRepository<User> studentRepo)
    {
        _db = db;
        _submittedAssignmentRepo = submittedAssignmentRepo;
        _assignmentRepo = assignmentRepo;
        _studentRepo = studentRepo;
    }

    public async Task<SubmittedAssignmentDTO> SubmitAssignmentAsync(SubmitAssignmentDTO dto)
    {
        var assignment = await _assignmentRepo.GetById(dto.AssignmentId);
        if (assignment == null)
        {
            throw new ArgumentException($"Assignment with ID '{dto.AssignmentId}' not found.");
        }

        var student = await _studentRepo.GetById(dto.StudentId);
        if (student == null)
        {
            throw new ArgumentException($"Student with ID '{dto.StudentId}' not found.");
        }

        var isStudentInGroup = await _db.GroupStudents
                                        .AnyAsync(gs => gs.GroupId == assignment.GroupId && gs.StudentId == dto.StudentId);
        if (!isStudentInGroup)
        {
            throw new UnauthorizedAccessException($"Student with ID '{dto.StudentId}' is not part of the group for Assignment with ID '{dto.AssignmentId}'.");
        }

        var existingSubmission = await _db.SubmittedAssignments
                                        .FirstOrDefaultAsync(sa => sa.AssignmentId == dto.AssignmentId && sa.StudentId == dto.StudentId);
        if (existingSubmission != null)
        {
            throw new InvalidOperationException($"Student with ID '{dto.StudentId}' has already submitted for Assignment with ID '{dto.AssignmentId}'.");
        }


        var submittedAssignment = new SubmittedAssignment
        {
            AssignmentId = dto.AssignmentId,
            StudentId = dto.StudentId,
            Title = dto.Title,
            SubmissionLink = dto.SubmissionLink,
            SubmissionDate = DateTime.UtcNow
        };

        await _submittedAssignmentRepo.Add(submittedAssignment);

        var addedSubmittedAssignment = await _db.SubmittedAssignments
            .Where(sa => sa.Id == submittedAssignment.Id)
            .Include(sa => sa.Assignment)
            .Include(sa => sa.Student)
            .FirstOrDefaultAsync();

        return MapToDto(addedSubmittedAssignment);
    }

    public async Task<IReadOnlyList<SubmittedAssignmentDTO>> GetInstructorSubmittedAssignmentsAsync(string instructorId)
    {
        var instructorGroups = await _db.Groups
                                        .Where(g => g.InstructorId == instructorId)
                                        .Select(g => g.Id)
                                        .ToListAsync();

        if (!instructorGroups.Any())
        {
            return new List<SubmittedAssignmentDTO>();
        }

        var submittedAssignments = await _db.SubmittedAssignments
            .Include(sa => sa.Assignment)
                .ThenInclude(a => a.Group)
            .Include(sa => sa.Student)
            .Where(sa => instructorGroups.Contains(sa.Assignment.GroupId))
            .AsNoTracking()
            .ToListAsync();

        return submittedAssignments.Select(sa => MapToDto(sa)).ToList();
    }

    public async Task<SubmittedAssignmentDTO> GradeSubmittedAssignmentAsync(string submittedAssignmentId, GradeSubmittedAssignmentDTO dto)
    {
        var submittedAssignment = await _db.SubmittedAssignments.FirstOrDefaultAsync(sa => sa.Id == submittedAssignmentId);

        if (submittedAssignment == null)
        {
            throw new ArgumentException($"Submitted Assignment with ID '{submittedAssignmentId}' not found.");
        }

        submittedAssignment.Grade = dto.Grade;
        submittedAssignment.InstructorNotes = dto.InstructorNotes;

        await _submittedAssignmentRepo.Update(submittedAssignment);

        var gradedSubmittedAssignment = await _db.SubmittedAssignments
            .Where(sa => sa.Id == submittedAssignment.Id)
            .Include(sa => sa.Assignment)
            .Include(sa => sa.Student)
            .FirstOrDefaultAsync();

        return MapToDto(gradedSubmittedAssignment);
    }

    public async Task<IReadOnlyList<SubmittedAssignmentDTO>> GetStudentSubmittedAssignmentsAsync(string studentId, string assignmentId = null)
    {
        IQueryable<SubmittedAssignment> query = _db.SubmittedAssignments
            .Include(sa => sa.Assignment)
            .Include(sa => sa.Student)
            .Where(sa => sa.StudentId == studentId);

        if (!string.IsNullOrEmpty(assignmentId))
        {
            query = query.Where(sa => sa.AssignmentId == assignmentId);
        }

        var submittedAssignments = await query.AsNoTracking().ToListAsync();
        return submittedAssignments.Select(sa => MapToDto(sa)).ToList();
    }

    public async Task<SubmittedAssignmentDTO> GetSubmittedAssignmentByIdAsync(string id)
    {
        var submittedAssignment = await _db.SubmittedAssignments
            .Include(sa => sa.Assignment)
            .Include(sa => sa.Student)
            .AsNoTracking()
            .FirstOrDefaultAsync(sa => sa.Id == id);

        if (submittedAssignment == null)
        {
            throw new ArgumentException($"Submitted Assignment with ID '{id}' not found.");
        }
        return MapToDto(submittedAssignment);
    }

    private SubmittedAssignmentDTO MapToDto(SubmittedAssignment submittedAssignment)
    {
        return new SubmittedAssignmentDTO
        {
            Id = submittedAssignment.Id,
            AssignmentId = submittedAssignment.AssignmentId,
            AssignmentTitle = submittedAssignment.Assignment?.Title ?? "N/A",
            StudentId = submittedAssignment.StudentId,
            StudentName = $"{submittedAssignment.Student?.FirstName} {submittedAssignment.Student?.LastName}".Trim(),
            SubmissionTitle = submittedAssignment.Title,
            SubmissionLink = submittedAssignment.SubmissionLink,
            SubmissionDate = submittedAssignment.SubmissionDate,
            Grade = submittedAssignment.Grade,
            InstructorNotes = submittedAssignment.InstructorNotes
        };
    }
}