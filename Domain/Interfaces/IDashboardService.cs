using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface IDashboardService
    {
        Task<DashboardDTO> GetForStudentAsync(string studentId);
    }
}
