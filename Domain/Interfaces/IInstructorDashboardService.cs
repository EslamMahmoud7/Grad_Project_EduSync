using Domain.DTOs;
using System.Threading.Tasks;

namespace Domain.Interfaces.IServices
{
    public interface IInstructorDashboardService
    {
        Task<InstructorDashboardCountsDTO> GetDashboardCountsAsync(string instructorId);
    }
}