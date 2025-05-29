using Domain.DTOs;
using Domain.Entities;
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
        private readonly ICourseService _courseService;
        public ProfileService(MainDbContext db, ICourseService courseService)
        {
            _courseService = courseService;
            _db = db;
        }
        public async Task<ProfileDTO> GetProfileById(string userId)
        {
            var courses = await _courseService.GetForStudent(userId);
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
                TotalCourses = courses.Count,
                GPA = u.GPA,
                Status = u.Status!,
                AvatarUrl = u.AvatarUrl!,
                Achievements = new List<string>(),
                RecentActivity = new List<string>(),
                SocialLinks = new List<string>()
            };
        }
        public async Task<ProfileDTO> UpdateProfile(UpdateProfileDTO dto,string Id)
        {
            var u = await _db.Users
                             .FirstOrDefaultAsync(x => x.Id == Id);
            if (u == null)
                throw new ArgumentException($"No user found with ID '{Id}'");

            u.FirstName = dto.FirstName;
            u.LastName = dto.LastName;
            u.PhoneNumber = dto.PhoneNumber;
            u.Institution = dto.Institution;
            u.AvatarUrl = dto.AvatarUrl;
            if (dto.TotalCourses.HasValue) u.TotalCourses = dto.TotalCourses.Value;
            if (dto.GPA.HasValue) u.GPA = dto.GPA.Value;
            u.Status = dto.Status;

            await _db.SaveChangesAsync();

            return Map(u);
        }

        private ProfileDTO Map(User u) => new ProfileDTO
        {
            Id = u.Id,
            FullName = $"{u.FirstName} {u.LastName}",
            Role = u.Role,
            Email = u.Email!,
            Phone = u.PhoneNumber ?? "",
            JoinedDate = u.JoinedDate.ToShortDateString(),
            Institution = u.Institution ?? "",
            TotalCourses = u.TotalCourses,
            GPA = u.GPA,
            Status = u.Status ?? "",
            AvatarUrl = u.AvatarUrl,
            Achievements = new List<string>(),
            RecentActivity = new List<string>(),
            SocialLinks = new List<string>()
        };
    }
}