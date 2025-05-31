using Domain.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces.IServices
{
    public interface IInstructorService
    {
        Task<InstructorDTO> AddInstructorAsync(CreateInstructorDTO dto);
        Task<InstructorDTO> GetInstructorByIdAsync(string id);
        Task<IReadOnlyList<InstructorDTO>> GetAllInstructorsAsync();
        Task<InstructorDTO> UpdateInstructorAsync(string id, UpdateInstructorDTO dto);
        Task DeleteInstructorAsync(string id);
    }
}