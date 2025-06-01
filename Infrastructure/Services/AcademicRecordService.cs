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
using System.Globalization; 
namespace Infrastructure.Services
{
    public class AcademicRecordService : IAcademicRecordService
    {
        private readonly IGenericRepository<AcademicRecord> _academicRecordRepo;
        private readonly MainDbContext _db;

        public AcademicRecordService(
            IGenericRepository<AcademicRecord> academicRecordRepo,
            MainDbContext db)
        {
            _academicRecordRepo = academicRecordRepo;
            _db = db;
        }

        public async Task<BulkAddAcademicRecordsResultDTO> AddAcademicRecordsFromCsvAsync(UploadAcademicRecordsCsvDTO uploadDto)
        {
            var result = new BulkAddAcademicRecordsResultDTO();
            var recordsToAdd = new List<AcademicRecord>();
            var parsedRows = new List<ParsedAcademicRecordCsvRowDTO>();

            var group = await _db.Groups.FindAsync(uploadDto.GroupId);
            if (group == null)
            {
                result.ErrorMessages.Add($"Group with ID '{uploadDto.GroupId}' not found.");
                return result;
            }

            User? uploaderInstructor = null;
            if (!string.IsNullOrEmpty(uploadDto.UploadingInstructorId))
            {
                uploaderInstructor = await _db.Users.FirstOrDefaultAsync(u => u.Id == uploadDto.UploadingInstructorId && u.Role == UserRole.Admin);
                if (uploaderInstructor == null)
                {
                    result.ErrorMessages.Add($"Uploading instructor (User) with ID '{uploadDto.UploadingInstructorId}' not found or is not an Instructor.");
                }
            }

            try
            {
                using (var reader = new StreamReader(uploadDto.CsvFile.OpenReadStream()))
                {
                    string? line;
                    int rowNumber = 0;
                    bool isFirstLine = true;

                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        rowNumber++;
                        if (isFirstLine)
                        {
                            isFirstLine = false;
                            continue;
                        }

                        if (string.IsNullOrWhiteSpace(line)) continue;

                        var values = line.Split(',');

                        if (values.Length < 2)
                        {
                            result.ErrorMessages.Add($"Row {rowNumber}: Invalid number of columns. Expected at least 2 (StudentId, GradeValue). Found {values.Length}. Line: '{line}'");
                            continue;
                        }

                        if (!int.TryParse(values[1].Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out int gradeValue))
                        {
                            result.ErrorMessages.Add($"Row {rowNumber}: Invalid GradeValue '{values[1].Trim()}'. Must be an integer. Line: '{line}'");
                            continue;
                        }

                        parsedRows.Add(new ParsedAcademicRecordCsvRowDTO
                        {
                            StudentId = values[0].Trim(),
                            GradeValue = gradeValue,
                            OriginalRowNumber = rowNumber
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessages.Add($"Error processing CSV file: {ex.Message}");
                return result;
            }

            result.TotalRowsAttempted = parsedRows.Count;
            if (!parsedRows.Any() && !result.ErrorMessages.Any(e => e.StartsWith("Error processing CSV file")))
            {
                result.ErrorMessages.Add("CSV file is empty or contains no valid data rows after the header.");
            }


            foreach (var row in parsedRows)
            {
                var student = await _db.Users.FirstOrDefaultAsync(u => u.Id == row.StudentId && u.Role == UserRole.Student);
                if (student == null)
                {
                    result.ErrorMessages.Add($"Row {row.OriginalRowNumber}: Student with ID '{row.StudentId}' not found or is not a student.");
                    continue;
                }

                var isStudentInGroup = await _db.GroupStudents.AnyAsync(gs => gs.GroupId == uploadDto.GroupId && gs.StudentId == row.StudentId);
                if (!isStudentInGroup)
                {
                    result.ErrorMessages.Add($"Row {row.OriginalRowNumber}: Student '{student.FirstName} {student.LastName}' (ID: {row.StudentId}) is not enrolled in group '{group.Label}' (ID: {uploadDto.GroupId}).");
                    continue;
                }

                if (row.GradeValue < 0 || row.GradeValue > 100)
                {
                    result.ErrorMessages.Add($"Row {row.OriginalRowNumber}: GradeValue {row.GradeValue} for student ID '{row.StudentId}' is out of range (0-100).");
                    continue;
                }

                recordsToAdd.Add(new AcademicRecord
                {
                    StudentId = row.StudentId,
                    GroupId = uploadDto.GroupId,
                    InstructorId = uploadDto.UploadingInstructorId,
                    GradeValue = row.GradeValue,
                    AssessmentType = uploadDto.AssessmentType,
                    Term = uploadDto.Term,
                    Status = uploadDto.DefaultStatus,
                    DateRecorded = DateTime.UtcNow
                });
            }

            if (recordsToAdd.Any())
            {
                try
                {
                    await _db.AcademicRecords.AddRangeAsync(recordsToAdd);
                    await _db.SaveChangesAsync();
                    result.SuccessfullyAddedCount = recordsToAdd.Count;
                }
                catch (Exception dbEx)
                {
                    result.ErrorMessages.Add($"Database error while saving records: {dbEx.Message}");
                    result.SuccessfullyAddedCount = 0;
                }
            }

            return result;
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
                .Include(ar => ar.Group).ThenInclude(g => g.Course)
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
                .Include(ar => ar.Group).ThenInclude(g => g.Course)
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
                .Include(ar => ar.Group).ThenInclude(g => g.Course)
                .Include(ar => ar.Instructor)
                .AsNoTracking()
                .OrderBy(ar => ar.Term).ThenBy(ar => ar.AssessmentType)
                .ToListAsync();
            return records.Select(MapToDto).ToList();
        }

        public async Task<IReadOnlyList<AcademicRecordDTO>> GetAcademicRecordsByGroupIdAsync(string groupId)
        {
            var records = await _db.AcademicRecords
                .Where(ar => ar.GroupId == groupId)
                .Include(ar => ar.Student)
                .Include(ar => ar.Group).ThenInclude(g => g.Course)
                .Include(ar => ar.Instructor)
                .AsNoTracking()
                .ToListAsync();
            return records.Select(MapToDto).ToList();
        }

        public async Task<AcademicRecordDTO> UpdateAcademicRecordAsync(string id, UpdateAcademicRecordDTO dto)
        {
            var record = await _db.AcademicRecords.FirstOrDefaultAsync(ar => ar.Id == id);
            if (record == null) throw new ArgumentException($"Academic Record with ID '{id}' not found.");

            if (dto.InstructorId != record.InstructorId)
            {
                if (!string.IsNullOrEmpty(dto.InstructorId))
                {
                    var instructorUser = await _db.Users
                        .FirstOrDefaultAsync(u => u.Id == dto.InstructorId && u.Role == UserRole.Instructor);
                    if (instructorUser == null)
                        throw new ArgumentException($"Instructor (User) with ID '{dto.InstructorId}' not found or is not an Instructor.");
                    record.InstructorId = dto.InstructorId;
                }
                else
                {
                    record.InstructorId = null;
                }
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
                InstructorFullName = record.Instructor != null ? $"{record.Instructor.FirstName} {record.Instructor.LastName}" : null,
                DateRecorded = record.DateRecorded,
                Status = record.Status,
                GradeValue = record.GradeValue,
                AssessmentType = record.AssessmentType
            };
        }
    }
}