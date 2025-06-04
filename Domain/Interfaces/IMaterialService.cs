using Domain.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces.IServices
{
    public interface IMaterialService
    {
        Task<MaterialDTO> AddMaterialAsync(CreateMaterialDTO dto);
        Task<MaterialDTO?> GetMaterialByIdAsync(string materialId);
        Task<IReadOnlyList<MaterialDTO>> GetMaterialsByGroupIdAsync(string groupId);
        Task<MaterialDTO> UpdateMaterialAsync(string materialId, UpdateMaterialDTO dto);
        Task<bool> DeleteMaterialAsync(string materialId);
    }
}