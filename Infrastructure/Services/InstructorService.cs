using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class InstructorService : IInstructorService
    {
        private readonly UserManager<User> _userManager;

        public InstructorService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<InstructorDTO> AddInstructorAsync(CreateInstructorDTO dto)
        {
            var existingUserByEmail = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUserByEmail != null)
            {

                throw new ArgumentException($"A user with email '{dto.Email}' already exists.");
            }

            var instructorUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                UserName = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Role = UserRole.Instructor,
                EmailConfirmed = true,
                JoinedDate = DateTime.UtcNow, 
            };

            if (string.IsNullOrEmpty(dto.Password))
            {
                throw new ArgumentException("Password is required to create an instructor user.");
            }
            var result = await _userManager.CreateAsync(instructorUser, dto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to create instructor user: {errors}");
            }

            return MapUserToInstructorDto(instructorUser);
        }

        public async Task DeleteInstructorAsync(string id)
        {
            var instructorUser = await _userManager.FindByIdAsync(id);
            if (instructorUser == null || instructorUser.Role != UserRole.Instructor)
            {
                throw new ArgumentException($"Instructor (User) with ID '{id}' not found or is not an Instructor.");
            }

            var result = await _userManager.DeleteAsync(instructorUser);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to delete instructor user: {errors}");
            }
        }

        public async Task<IReadOnlyList<InstructorDTO>> GetAllInstructorsAsync()
        {
            var instructorUsers = await _userManager.Users
                                        .Where(u => u.Role == UserRole.Instructor)
                                        .ToListAsync();
            return instructorUsers.Select(MapUserToInstructorDto).ToList();
        }

        public async Task<InstructorDTO> GetInstructorByIdAsync(string id)
        {
            var instructorUser = await _userManager.FindByIdAsync(id);
            if (instructorUser == null || instructorUser.Role != UserRole.Instructor)
            {
                throw new ArgumentException($"Instructor (User) with ID '{id}' not found or is not an Instructor.");
            }
            return MapUserToInstructorDto(instructorUser);
        }

        public async Task<InstructorDTO> UpdateInstructorAsync(string id, UpdateInstructorDTO dto)
        {
            var instructorUser = await _userManager.FindByIdAsync(id);
            if (instructorUser == null || instructorUser.Role != UserRole.Instructor)
            {
                throw new ArgumentException($"Instructor (User) with ID '{id}' not found or is not an Instructor.");
            }

            if (!string.IsNullOrEmpty(dto.Email) && dto.Email != instructorUser.Email)
            {
                var existingUserWithNewEmail = await _userManager.FindByEmailAsync(dto.Email);
                if (existingUserWithNewEmail != null && existingUserWithNewEmail.Id != id)
                {
                    throw new ArgumentException($"Another user with email '{dto.Email}' already exists.");
                }
                instructorUser.Email = dto.Email;
                instructorUser.UserName = dto.Email;
            }

            if (dto.FirstName != null) instructorUser.FirstName = dto.FirstName;
            if (dto.LastName != null) instructorUser.LastName = dto.LastName;
            if (dto.PhoneNumber != null) instructorUser.PhoneNumber = dto.PhoneNumber;

            var result = await _userManager.UpdateAsync(instructorUser);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to update instructor user: {errors}");
            }
            return MapUserToInstructorDto(instructorUser);
        }

        private InstructorDTO MapUserToInstructorDto(User user)
        {
            return new InstructorDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }
    }
}