// Domain/Interfaces/IServices/ICourseService.cs
using Domain.DTOs;
using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces.IServices
{
    public interface ICourseService
    {
        Task<CourseDto> Add(CourseDto dto);
        Task Delete(string id);
        Task<CourseDto> Get(string id);
        Task<IReadOnlyList<CourseDto>> GetAll();
        Task<CourseDto> Update(CourseDto dto, string CourseID);
        Task<IReadOnlyList<CourseDto>> GetForStudent(string studentId);
        Task AssignCourseAsync(string studentId, string courseId);

        Task AssignStudentToGroupAsync(GroupEnrollmentDTO dto);
    }
}