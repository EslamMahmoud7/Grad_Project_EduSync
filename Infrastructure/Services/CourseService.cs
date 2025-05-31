using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.IServices;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Services
{
    public class CourseService : ICourseService
    {
        private readonly IGenericRepository<Course> _courseRepo;
        private readonly IPaginationService _paginationService;
        private readonly IGenericRepository<StudentCourse> _studentCourseRepo;
        private readonly IGenericRepository<GroupStudent> _groupStudentRepo;
        private readonly MainDbContext _db;


        public CourseService(
            IGenericRepository<Course> courseRepo,
            IPaginationService paginationService,
            IGenericRepository<StudentCourse> studentCourseRepo,
            IGenericRepository<GroupStudent> groupStudentRepo, MainDbContext dbContext)
        {
            _courseRepo = courseRepo;
            _paginationService = paginationService;
            _studentCourseRepo = studentCourseRepo;
            _groupStudentRepo = groupStudentRepo;
            _db = dbContext;
        }

        public async Task<CourseDto> Add(CourseDto dto)
        {
            var course = new Course
            {
                Id = Guid.NewGuid().ToString(),
                Code = dto.Code,
                Title = dto.Title,
                Description = dto.Description,
                InstructorName = dto.InstructorName,
                InstructorEmail = dto.InstructorEmail,
                ResourceLink = dto.ResourceLink,
                Credits = dto.Credits,
                Progress = dto.Progress,
                NextDeadline = dto.NextDeadline,
                Level = dto.Level
            };

            await _courseRepo.Add(course);
            return MapToDto(course);
        }

        public async Task Delete(string id)
        {
            var course = await _courseRepo.GetById(id);
            if (course == null) throw new ArgumentException("Course not found");

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

        public async Task<CourseDto> Update(CourseDto dto, string CourseID)
        {
            var course = await _courseRepo.GetById(CourseID);
            if (course == null) throw new ArgumentException("Course not found");

            course.Code = dto.Code;
            course.Title = dto.Title;
            course.Description = dto.Description;
            course.InstructorName = dto.InstructorName;
            course.InstructorEmail = dto.InstructorEmail;
            course.ResourceLink = dto.ResourceLink;
            course.Credits = dto.Credits;
            course.Progress = dto.Progress;
            course.NextDeadline = dto.NextDeadline;
            course.Level = dto.Level;
            await _courseRepo.Update(course);
            return MapToDto(course);
        }

        private CourseDto MapToDto(Course course)
        {
            return new CourseDto
            {
                Code = course.Code,
                Title = course.Title,
                Description = course.Description,
                InstructorName = course.InstructorName,
                InstructorEmail = course.InstructorEmail,
                ResourceLink = course.ResourceLink,
                Credits = course.Credits,
                Progress = course.Progress,
                Level = course.Level,
                NextDeadline = course.NextDeadline
            };
        }
        public async Task<IReadOnlyList<CourseDto>> GetForStudent(string studentId)
        {
            var enrollments = await _studentCourseRepo.GetAll();
            var mine = enrollments
                .Where(sc => sc.StudentId == studentId)
                .Select(sc => sc.CourseId)
                .ToList();

            var courses = new List<Course>();
            foreach (var courseId in mine)
            {
                var c = await _courseRepo.GetById(courseId);
                if (c != null)
                    courses.Add(c);
            }

            return courses.Select(MapToDto).ToList();
        }
        public async Task AssignCourseAsync(string studentId, string courseId)
        {
            var allEnrollments = await _studentCourseRepo.GetAll();
            if (allEnrollments.Any(sc =>
                    sc.StudentId == studentId &&
                    sc.CourseId == courseId))
            {
                throw new ArgumentException("Student is already enrolled in this course");
            }
            var enrollment = new StudentCourse
            {
                StudentId = studentId,
                CourseId = courseId
            };

            await _studentCourseRepo.Add(enrollment);
        }

        public async Task AssignStudentToGroupAsync(GroupEnrollmentDTO dto)
        {
            var group = await _db.Groups.FindAsync(dto.GroupId);
            if (group == null)
            {
                throw new ArgumentException($"Group with ID '{dto.GroupId}' not found.");
            }

            var existingEnrollment = await _groupStudentRepo.GetAll();
            if (existingEnrollment.Any(gs =>
                    gs.StudentId == dto.StudentId &&
                    gs.GroupId == dto.GroupId))
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
    }
}