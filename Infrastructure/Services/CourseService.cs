using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.IServices;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace Infrastructure.Services
{
    public class CourseService : ICourseService
    {
        private readonly IGenericRepository<Course> _courseRepo;
        private readonly IPaginationService _paginationService;

        public CourseService(IGenericRepository<Course> courseRepo, IPaginationService paginationService)
        {
            _courseRepo = courseRepo;
            _paginationService = paginationService;
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
    }
}
