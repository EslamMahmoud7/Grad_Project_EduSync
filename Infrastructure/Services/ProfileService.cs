using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces.IServices;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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
                .Include(u => u.StudentProfile)
                .Include(u => u.AdminProfile)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new ArgumentException("User not found");

            if (user.Role == UserRole.Student)
            {
                var student = user.StudentProfile;
                return new ProfileDTO
                {
                    Id = user.Id,
                    FullName = user.UserName,
                    Role = "Student",
                    Email = user.Email,
                    Phone = user.PhoneNumber,
                    JoinedDate = student.JoinedDate.ToShortDateString(),
                    Institution = student.Institution,
                    GPA = student.GPA,
                    Status = student.Status,
                    AvatarUrl = student.AvatarUrl,
                    TotalCourses = student.TotalCourses,
                    Achievements = new List<string>(),
                    RecentActivity = new List<string>(),
                    SocialLinks = new List<string>()
                };
            }
            else if (user.Role == UserRole.Admin)
            {
                var admin = user.AdminProfile;
                return new ProfileDTO
                {
                    Id = user.Id,
                    FullName = user.UserName,
                    Role = "Admin",
                    Email = user.Email,
                    Phone = user.PhoneNumber,
                    JoinedDate = user.CreatedAt.ToShortDateString(),
                    Institution = admin.Department,
                    GPA = 0,
                    Status = "N/A",
                    AvatarUrl = admin.AvatarUrl,
                    TotalCourses = 0,
                    Achievements = new List<string>(),
                    RecentActivity = new List<string>(),
                    SocialLinks = new List<string>()
                };
            }

            throw new Exception("Unknown role");
        }
    }
}
