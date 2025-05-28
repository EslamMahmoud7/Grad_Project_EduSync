using Domain.DTOs;
using System.Threading.Tasks;

namespace Domain.Interfaces.IServices
{
    public interface IProfileService
    {
        Task<ProfileDTO> GetProfileById(string userId);
    }
}