using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces.IServices;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class AcademicRecordService : IAcademicRecordService
    {
        private readonly IGenericRepository<AcademicRecord> _academicRecordRepo;
        private readonly MainDbContext _db;

        public AcademicRecordService(IGenericRepository<AcademicRecord> academicRecordRepo, MainDbContext db)
        {
            _academicRecordRepo = academicRecordRepo;
            _db = db;
        }

        public async Task<AcademicRecordDTO> AddAcademicRecordAsync(CreateAcademicRecordDTO dto)
        {
            var student = await _db.Users.FindAsync(dto.StudentId);
            if (student == null) throw new ArgumentException($"Student with ID '{dto.StudentId}' not found.");

            var group = await _db.Groups.FindAsync(dto.GroupId);
            if (group == null) throw new ArgumentException($"Group with ID '{dto.GroupId}' not found.");

            Instructor? instructor = null;
            if (!string.IsNullOrEmpty(dto.InstructorId))
            {
                instructor = await _db.Instructors.FindAsync(dto.InstructorId);
                if (instructor == null) throw new ArgumentException($"Instructor with ID '{dto.InstructorId}' not found.");
            }

            var record = new AcademicRecord
            {
                Id = Guid.NewGuid().ToString(),
                StudentId = dto.StudentId,
                GroupId = dto.GroupId,
                InstructorId = dto.InstructorId,
                GradeValue = dto.GradeValue,
                AssessmentType = dto.AssessmentType,
                Term = dto.Term,
                Status = dto.Status,
                DateRecorded = DateTime.UtcNow
            };

            await _academicRecordRepo.Add(record);

            return await GetAcademicRecordByIdAsync(record.Id);
        }

        public async Task DeleteAcademicRecordAsync(string id)
        {
            var record = await _academicRecordRepo.GetById(id);
            if (record == null) throw new ArgumentException($"Academic Record with ID '{id}' not found.");
            await _academicRecordRepo.Delete(record);
        }

        public async Task<AcademicRecordDTO> GetAcademicRecordByIdAsync(string id)
        {
            var record = await _db.AcademicRecords
                .Include(ar => ar.Student)
                .Include(ar => ar.Group)
                    .ThenInclude(g => g.Course)
                .Include(ar => ar.Instructor)
                .AsNoTracking()
                .FirstOrDefaultAsync(ar => ar.Id == id);

            if (record == null) throw new ArgumentException($"Academic Record with ID '{id}' not found.");
            return MapToDto(record);
        }

        public async Task<IReadOnlyList<AcademicRecordDTO>> GetAllAcademicRecordsAsync()
        {
            var records = await _db.AcademicRecords
                .Include(ar => ar.Student)
                .Include(ar => ar.Group)
                    .ThenInclude(g => g.Course)
                .Include(ar => ar.Instructor)
                .AsNoTracking()
                .ToListAsync();

            return records.Select(MapToDto).ToList();
        }

        public async Task<IReadOnlyList<AcademicRecordDTO>> GetAcademicRecordsByStudentIdAsync(string studentId)
        {
            var records = await _db.AcademicRecords
                .Where(ar => ar.StudentId == studentId)
                .Include(ar => ar.Student)
                .Include(ar => ar.Group)
                    .ThenInclude(g => g.Course)
                .Include(ar => ar.Instructor)
                .AsNoTracking()
                .OrderBy(ar => ar.Term)
                .ThenBy(ar => ar.AssessmentType)
                .ToListAsync();

            return records.Select(MapToDto).ToList();
        }

        public async Task<IReadOnlyList<AcademicRecordDTO>> GetAcademicRecordsByGroupIdAsync(string groupId)
        {
            var records = await _db.AcademicRecords
                .Where(ar => ar.GroupId == groupId)
                .Include(ar => ar.Student)
                .Include(ar => ar.Group)
                    .ThenInclude(g => g.Course)
                .Include(ar => ar.Instructor)
                .AsNoTracking()
                .ToListAsync();

            return records.Select(MapToDto).ToList();
        }

        public async Task<AcademicRecordDTO> UpdateAcademicRecordAsync(string id, UpdateAcademicRecordDTO dto)
        {
            var record = await _db.AcademicRecords.FirstOrDefaultAsync(ar => ar.Id == id);
            if (record == null) throw new ArgumentException($"Academic Record with ID '{id}' not found.");

            if (!string.IsNullOrEmpty(dto.InstructorId) && dto.InstructorId != record.InstructorId)
            {
                var instructor = await _db.Instructors.FindAsync(dto.InstructorId);
                if (instructor == null) throw new ArgumentException($"Instructor with ID '{dto.InstructorId}' not found.");
                record.InstructorId = dto.InstructorId;
            }
            else if (string.IsNullOrEmpty(dto.InstructorId))
            {
                record.InstructorId = null;
            }

            if (dto.GradeValue.HasValue) record.GradeValue = dto.GradeValue.Value;
            if (dto.AssessmentType.HasValue) record.AssessmentType = dto.AssessmentType.Value;

            if (dto.Term != null) record.Term = dto.Term;
            if (dto.Status.HasValue) record.Status = dto.Status.Value;
            if (dto.DateRecorded.HasValue) record.DateRecorded = dto.DateRecorded.Value;

            await _academicRecordRepo.Update(record);

            return await GetAcademicRecordByIdAsync(record.Id);
        }

        private AcademicRecordDTO MapToDto(AcademicRecord record)
        {
            return new AcademicRecordDTO
            {
                Id = record.Id,
                StudentId = record.StudentId,
                StudentFullName = $"{record.Student?.FirstName} {record.Student?.LastName}",
                GroupId = record.GroupId,
                GroupLabel = record.Group?.Label ?? "N/A",
                CourseId = record.Group?.Course?.Id ?? "N/A",
                CourseCode = record.Group?.Course?.Code ?? "N/A",
                CourseTitle = record.Group?.Course?.Title ?? "N/A",
                Term = record.Term,
                InstructorId = record.InstructorId,
                InstructorFullName = $"{record.Instructor?.FirstName} {record.Instructor?.LastName}",
                DateRecorded = record.DateRecorded,
                Status = record.Status,
                GradeValue = record.GradeValue,
                AssessmentType = record.AssessmentType
            };
        }
    }
}