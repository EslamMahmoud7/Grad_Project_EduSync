using Domain.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ICourseScheduleService
{
    Task<CourseScheduleDTO> AddAsync(CreateCourseScheduleDTO dto);
    Task<IReadOnlyList<CourseScheduleDTO>> GetForStudentAsync(string studentId);
}
