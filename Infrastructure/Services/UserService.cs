using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces.IServices;
using Grad_Project_LMS.Controller;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IReadOnlyList<UserDTO>> GetAllStudentsAsync()
        {
            var studentUsers = await _userManager.Users
                                     .Where(user => user.Role == UserRole.Student)
                                     .ToListAsync();

            var studentDTOs = studentUsers
                .Select(user => new UserDTO
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email ?? "N/A",
                    Role = user.Role
                })
                .ToList();

            return studentDTOs;
        }
    }
}