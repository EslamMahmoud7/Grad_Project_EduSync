// Infrastructure/Services/ProfileService.cs
using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces.IServices;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ProfileService : IProfileService
    {
        private readonly MainDbContext _db;

        public ProfileService(MainDbContext db)
        {
            _db = db;
        }

        public async Task<ProfileDTO> GetProfileById(string userId)
        {
            var user = await _db.Users
                .AsNoTracking()
                .Include(u => u.GroupStudents)
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
                throw new ArgumentException($"No user found with ID '{userId}'");

            var totalCourses = await _db.GroupStudents
                .Where(gs => gs.StudentId == userId)
                .Select(gs => gs.Group.CourseId)
                .Distinct()
                .CountAsync();

            return new ProfileDTO
            {
                Id = user.Id,
                FullName = $"{user.FirstName} {user.LastName}",
                Role = user.Role,
                Email = user.Email!,
                Phone = user.PhoneNumber!,
                JoinedDate = user.JoinedDate,
                Institution = user.Institution!,
                TotalCourses = totalCourses,
                GPA = user.GPA,
                Status = user.Status!,
                AvatarUrl = user.AvatarUrl!,
                Achievements = new List<string>(),
                RecentActivity = new List<string>(),
                SocialLinks = new List<string>()
            };
        }

        public async Task<ProfileDTO> UpdateProfile(UpdateProfileDTO dto, string Id)
        {
            var user = await _db.Users
                             .FirstOrDefaultAsync(x => x.Id == Id);
            if (user == null)
                throw new ArgumentException($"No user found with ID '{Id}'");

            if (dto.FirstName != null) user.FirstName = dto.FirstName;
            if (dto.LastName != null) user.LastName = dto.LastName;
            if (dto.PhoneNumber != null) user.PhoneNumber = dto.PhoneNumber;
            if (dto.Institution != null) user.Institution = dto.Institution;
            if (dto.Status != null) user.Status = dto.Status;
            if (dto.AvatarUrl != null) user.AvatarUrl = dto.AvatarUrl;

            await _db.SaveChangesAsync();

            return await GetProfileById(Id);
        }

    }
}