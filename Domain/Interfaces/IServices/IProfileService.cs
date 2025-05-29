using Domain.DTOs;
using Domain.Entities;
using System.Threading.Tasks;

namespace Domain.Interfaces.IServices
{
    public interface IProfileService
    {
        Task<ProfileDTO> GetProfileById(string userId);
        Task<ProfileDTO> UpdateProfile(UpdateProfileDTO dto, string userId);
    }
}