using Domain.DTOs;
using System.Threading.Tasks;

namespace Domain.Interfaces.IServices
{
    public interface IAdminDashboardService
    {
        Task<AdminDashboardCountsDTO> GetDashboardCountsAsync();
    }
}