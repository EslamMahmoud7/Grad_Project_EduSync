using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces.IServices;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class InstructorDashboardService : IInstructorDashboardService
    {
        private readonly MainDbContext _context;

        public InstructorDashboardService(MainDbContext context)
        {
            _context = context;
        }

        public async Task<InstructorDashboardCountsDTO> GetDashboardCountsAsync(string instructorId)
        {
            var instructorExists = await _context.Users
                                         .AnyAsync(u => u.Id == instructorId && u.Role == UserRole.Instructor);

            if (!instructorExists)
            {
                throw new ArgumentException("Instructor not found or ID does not correspond to an instructor role.");
            }

            var instructorGroupIds = await _context.Groups
                .Where(g => g.InstructorId == instructorId)
                .Select(g => g.Id)
                .ToListAsync();

            int totalGroups = instructorGroupIds.Count;

            int totalAssignmentsInGroups = 0;
            if (totalGroups > 0)
            {
                totalAssignmentsInGroups = await _context.Assignments
                    .CountAsync(a => instructorGroupIds.Contains(a.GroupId));
            }

            DateTime today = DateTime.UtcNow.Date;
            int totalTodayClasses = 0;
            if (totalGroups > 0)
            {
                totalTodayClasses = await _context.Groups
                   .CountAsync(g => g.InstructorId == instructorId && g.StartTime.Date == today);
            }


            int totalUniqueStudentsInGroups = 0;
            if (totalGroups > 0)
            {
                totalUniqueStudentsInGroups = await _context.GroupStudents
                    .Where(gs => instructorGroupIds.Contains(gs.GroupId))
                    .Select(gs => gs.StudentId)
                    .Distinct()
                    .CountAsync();
            }

            return new InstructorDashboardCountsDTO
            {
                TotalGroups = totalGroups,
                TotalAssignmentsInGroups = totalAssignmentsInGroups,
                TotalTodayClasses = totalTodayClasses,
                TotalUniqueStudentsInGroups = totalUniqueStudentsInGroups
            };
        }
    }
}