using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces.IServices;
using Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class CourseService : ICourseService
    {
        private readonly IGenericRepository<Course> _courseGenericRepo;
        public CourseService(IGenericRepository<Course> CourseGenericRepo)
        {
            _courseGenericRepo = CourseGenericRepo;
        }
        public async Task<CourseDTO> Add(CourseDTO courseDTO)
        {
            var Course = new Course()
            {
                CourseId = courseDTO.CourseId,
                CourseName = courseDTO.CourseName,
                CourseDescription = courseDTO.CourseDescription,
                CourseProgress = courseDTO.CourseProgress,
                CourseTitle = courseDTO.CourseTitle,
                CreatedAt = DateTime.Now,
                CreatedBy = "super admin"
            };
            await _courseGenericRepo.Add(Course);
            return MapToCourseDTO(Course);
        }
        public async Task Delete(int id)
        {
            var Course = await _courseGenericRepo.GetById(id);
            if (Course == null) throw new ArgumentException("course not exist");
            await _courseGenericRepo.Delete(Course);
        }
        public async Task<CourseDTO> Get(int id)
        {
            var Course = await _courseGenericRepo.GetById(id);
            if (Course == null) throw new ArgumentException("Course is null");
            return MapToCourseDTO(Course);
        }
        public async Task<IReadOnlyList<CourseDTO>> GetAll()
        {
            var Courses = await _courseGenericRepo.GetAll();
            return Courses.Select(MapToCourseDTO).ToList();
        }
        public async Task<CourseDTO> Update(CourseDTO courseDTO)
        {
            var Course = await _courseGenericRepo.GetById(courseDTO.CourseId);
            if (Course == null) throw new ArgumentException("course is null");
            Course.CourseId = courseDTO.CourseId;
            Course.CourseName = courseDTO.CourseName;
            Course.CourseDescription = courseDTO.CourseDescription;
            Course.CourseTitle = courseDTO.CourseTitle;
            Course.CreatedAt = DateTime.Now;
            Course.CreatedBy = "Super admin";
            await _courseGenericRepo.Update(Course);
            return MapToCourseDTO(Course);
        }
        public CourseDTO MapToCourseDTO(Course course)
        {
            return new CourseDTO()
            {
                CourseId = course.CourseId,
                CourseName = course.CourseName,
                CourseDescription = course.CourseDescription,
                CourseProgress = course.CourseProgress,
                CourseTitle = course.CourseTitle
            };
        }
    }
}
