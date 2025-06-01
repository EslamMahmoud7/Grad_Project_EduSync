using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces.IServices;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly MainDbContext _context;
        private readonly UserManager<User> _userManager;

        public AdminDashboardService(MainDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<AdminDashboardCountsDTO> GetDashboardCountsAsync()
        {
            int totalCourses = await _context.Courses.CountAsync();
            int totalGroups = await _context.Groups.CountAsync();

            int totalStudents = await _userManager.Users.CountAsync(u => u.Role == UserRole.Student);
            int totalInstructors = await _userManager.Users.CountAsync(u => u.Role == UserRole.Instructor);


            return new AdminDashboardCountsDTO
            {
                TotalCourses = totalCourses,
                TotalGroups = totalGroups,
                TotalStudents = totalStudents,
                TotalInstructors = totalInstructors
            };
        }
    }
}