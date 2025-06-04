using Domain.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ICourseScheduleService
{
    Task<IReadOnlyList<CourseScheduleDTO>> GetForStudentAsync(string studentId);
    Task<IReadOnlyList<CourseScheduleDTO>> GetForInstructorAsync(string instructorId);
}
