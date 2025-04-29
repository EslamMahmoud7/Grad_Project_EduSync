using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces;
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
        private readonly IPaginationService _paginationService;

        public CourseService(IGenericRepository<Course> CourseGenericRepo, IPaginationService paginationService)
        {
            _courseGenericRepo = CourseGenericRepo;
            _paginationService = paginationService;
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

        public async Task<PaginatedResultDTO<CourseDTO>> GetAllPaginated(int pagenumber = 1, int pagesize = 3)
        {
            var Courses = await _courseGenericRepo.GetAll();
            var IQuerableCoursed = Courses.AsQueryable();

            var CourseDTO = IQuerableCoursed.Select(c => MapToCourseDTO(c)).ToList().AsQueryable();

            var PaginatedResult = _paginationService.Paginate(CourseDTO, pagenumber, pagesize);
            return PaginatedResult;
        }
    }
}
