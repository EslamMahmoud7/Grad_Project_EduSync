using Domain.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces.IServices
{
    public interface IAcademicRecordService
    {
        Task<AcademicRecordDTO> AddAcademicRecordAsync(CreateAcademicRecordDTO dto);
        Task<AcademicRecordDTO> GetAcademicRecordByIdAsync(string id);
        Task<IReadOnlyList<AcademicRecordDTO>> GetAllAcademicRecordsAsync();
        Task<IReadOnlyList<AcademicRecordDTO>> GetAcademicRecordsByStudentIdAsync(string studentId);
        Task<IReadOnlyList<AcademicRecordDTO>> GetAcademicRecordsByGroupIdAsync(string groupId);
        Task<AcademicRecordDTO> UpdateAcademicRecordAsync(string id, UpdateAcademicRecordDTO dto);
        Task DeleteAcademicRecordAsync(string id);
    }
}