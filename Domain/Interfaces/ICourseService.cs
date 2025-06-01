// Domain/Interfaces/IServices/ICourseService.cs
using Domain.DTOs;
using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces.IServices
{
    public interface ICourseService
    {
        Task<CourseDto> Add(CreateCourseDto dto);
        Task Delete(string id);
        Task<CourseDto> Get(string id);
        Task<IReadOnlyList<CourseDto>> GetAll();
        Task<CourseDto> Update(UpdateCourseDto dto, string CourseID);
        Task AssignStudentToGroupAsync(GroupEnrollmentDTO dto);
        Task<BulkEnrollmentResultDTO> AssignStudentsToGroupBulkAsync(BulkGroupEnrollmentDTO dto);
    }
}