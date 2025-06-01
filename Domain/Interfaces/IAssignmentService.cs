using Domain.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces.IServices
{
    public interface IAssignmentService
    {
        Task<AssignmentDTO> AddAssignmentAsync(CreateAssignmentDTO dto);
        Task<IReadOnlyList<AssignmentDTO>> GetForStudentAsync(string studentId);
        Task<AssignmentDTO> GetAssignmentByIdAsync(string id);
        Task<IReadOnlyList<AssignmentDTO>> GetAllAssignmentsAsync();
        Task<AssignmentDTO> UpdateAssignmentAsync(string id, UpdateAssignmentDTO dto); 
        Task DeleteAssignmentAsync(string id);
    }
}