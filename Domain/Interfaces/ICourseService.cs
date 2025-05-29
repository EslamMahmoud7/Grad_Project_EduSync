using Domain.DTOs;

namespace Domain.Interfaces.IServices
{
    public interface ICourseService
    {
        Task<CourseDto> Add(CourseDto courseDTO);
        Task<CourseDto> Update(CourseDto courseDTO,string courseId);
        Task Delete(string id);
        Task<CourseDto> Get(string id);
        Task<IReadOnlyList<CourseDto>> GetAll();
        Task<IReadOnlyList<CourseDto>> GetForStudent(string studentId);
        Task AssignCourseAsync(string studentId, string courseId);
    }
}
