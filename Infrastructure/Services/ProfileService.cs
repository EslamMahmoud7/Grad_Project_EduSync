// Infrastructure/Services/ProfileService.cs

using Domain.DTOs;
using Domain.Interfaces.IServices;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ProfileService : IProfileService
    {
        private readonly MainDbContext _db;
        public ProfileService(MainDbContext db) => _db = db;

        public async Task<ProfileDTO> GetProfileById(string userId)
        {
            var u = await _db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (u == null)
                throw new ArgumentException($"No user found with ID '{userId}'");

            return new ProfileDTO
            {
                Id = u.Id,
                FullName = $"{u.FirstName} {u.LastName}",
                Role = u.Role,
                Email = u.Email!,
                Phone = u.PhoneNumber!,
                JoinedDate = u.JoinedDate.ToShortDateString(),
                Institution = u.Institution!,
                TotalCourses = u.TotalCourses,
                GPA = u.GPA,
                Status = u.Status!,
                AvatarUrl = u.AvatarUrl!,
                Achievements = new List<string>(),
                RecentActivity = new List<string>(),
                SocialLinks = new List<string>()
            };
        }
    }
}