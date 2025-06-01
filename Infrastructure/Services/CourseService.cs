using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.IServices;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Infrastructure.Services
{
    public class CourseService : ICourseService
    {
        private readonly IGenericRepository<Course> _courseRepo;
        private readonly IPaginationService _paginationService;
        private readonly IGenericRepository<GroupStudent> _groupStudentRepo;
        private readonly MainDbContext _db;

        public CourseService(
            IGenericRepository<Course> courseRepo,
            IPaginationService paginationService,
            IGenericRepository<GroupStudent> groupStudentRepo,
            MainDbContext db)
        {
            _courseRepo = courseRepo;
            _paginationService = paginationService;
            _groupStudentRepo = groupStudentRepo;
            _db = db;
        }

        public async Task<CourseDto> Add(CreateCourseDto dto)
        {
            var course = new Course
            {
                Id = Guid.NewGuid().ToString(),
                Code = dto.Code,
                Title = dto.Title,
                Description = dto.Description,
                ResourceLink = dto.ResourceLink,
                Level = dto.Level
            };

            await _courseRepo.Add(course);
            return MapToDto(course);
        }

        public async Task Delete(string id)
        {
            var course = await _courseRepo.GetById(id);
            if (course == null) throw new ArgumentException("Course not found");

            var linkedGroups = await _db.Groups.AnyAsync(g => g.CourseId == id);
            if (linkedGroups)
            {
                throw new InvalidOperationException("Cannot delete course while active groups are linked to it. Delete groups first.");
            }

            await _courseRepo.Delete(course);
        }

        public async Task<CourseDto> Get(string id)
        {
            var course = await _courseRepo.GetById(id);
            if (course == null) throw new ArgumentException("Course not found");

            return MapToDto(course);
        }

        public async Task<IReadOnlyList<CourseDto>> GetAll()
        {
            var courses = await _courseRepo.GetAll();
            return courses.Select(MapToDto).ToList();
        }

        public async Task<CourseDto> Update(UpdateCourseDto dto, string CourseID)
        {
            var course = await _courseRepo.GetById(CourseID);
            if (course == null) throw new ArgumentException("Course not found");

            if (dto.Code != null) course.Code = dto.Code;
            if (dto.Title != null) course.Title = dto.Title;
            if (dto.Description != null) course.Description = dto.Description;
            if (dto.ResourceLink != null) course.ResourceLink = dto.ResourceLink;
            if (dto.Credits.HasValue) course.Credits = dto.Credits.Value;
            if (dto.Level.HasValue) course.Level = dto.Level.Value;

            await _courseRepo.Update(course);
            return MapToDto(course);
        }

        private CourseDto MapToDto(Course course)
        {
            return new CourseDto
            {
                Id = course.Id,
                Code = course.Code,
                Title = course.Title,
                Description = course.Description,
                Credits = course.Credits,
                ResourceLink = course.ResourceLink,
                Level = course.Level,
            };
        }
        public async Task AssignStudentToGroupAsync(GroupEnrollmentDTO dto)
        {
            var group = await _db.Groups.FindAsync(dto.GroupId);
            if (group == null)
            {
                throw new ArgumentException($"Group with ID '{dto.GroupId}' not found.");
            }

            var existingEnrollment = await _db.GroupStudents
                .AsNoTracking()
                .AnyAsync(gs => gs.StudentId == dto.StudentId && gs.GroupId == dto.GroupId);

            if (existingEnrollment)
            {
                throw new ArgumentException("Student is already enrolled in this group.");
            }

            var groupStudent = new GroupStudent
            {
                StudentId = dto.StudentId,
                GroupId = dto.GroupId
            };

            await _groupStudentRepo.Add(groupStudent);
        }
    public async Task<BulkEnrollmentResultDTO> AssignStudentsToGroupBulkAsync(BulkGroupEnrollmentDTO dto)
        {
            var result = new BulkEnrollmentResultDTO
            {
                GroupId = dto.GroupId,
                TotalStudentsProcessed = dto.StudentIds.Count
            };

            var group = await _db.Groups.FindAsync(dto.GroupId);
            if (group == null)
            {
                result.Errors["_global"] = $"Group with ID '{dto.GroupId}' not found.";
                return result;
            }

            var existingEnrollmentsInGroup = await _db.GroupStudents
                .Where(gs => gs.GroupId == dto.GroupId)
                .Select(gs => gs.StudentId)
                .ToListAsync();

            foreach (var studentId in dto.StudentIds.Distinct())
            {
                var student = await _db.Users.FindAsync(studentId);
                if (student == null)
                {
                    result.FailedStudentIds.Add(studentId);
                    result.Errors[studentId] = "Student not found.";
                    continue;
                }

                if (existingEnrollmentsInGroup.Contains(studentId))
                {
                    result.FailedStudentIds.Add(studentId);
                    result.Errors[studentId] = "Already enrolled in this group.";
                    continue;
                }

                var groupStudent = new GroupStudent
                {
                    StudentId = studentId,
                    GroupId = dto.GroupId
                };

                _db.GroupStudents.Add(groupStudent);
                result.StudentsEnrolledSuccessfully++;
            }

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                result.Errors["_global"] = $"Database save failed: {ex.Message}";
                foreach (var studentId in dto.StudentIds.Except(result.FailedStudentIds))
                {
                    result.FailedStudentIds.Add(studentId);
                    result.Errors[studentId] = "Database save failed.";
                }
            }

            return result;
        }
    }
}