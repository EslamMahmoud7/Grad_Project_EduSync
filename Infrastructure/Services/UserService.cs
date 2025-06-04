using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization; // For CSV parsing if needed

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        private UserDTO MapUserToUserDTO(User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                FirstName = user.FirstName ?? "",
                LastName = user.LastName ?? "",
                Email = user.Email ?? "N/A",
                Role = user.Role,
                IsActive = user.Status == "Active", 
                CreatedAt = user.JoinedDate,
                AvatarUrl = user.AvatarUrl,
                PhoneNumber = user.PhoneNumber,
                Institution = user.Institution
            };
        }

        public async Task<IReadOnlyList<UserDTO>> GetAllUsersAsync(UserRole? roleFilter = null)
        {
            var query = _userManager.Users;

            if (roleFilter.HasValue)
            {
                query = query.Where(user => user.Role == roleFilter.Value);
            }

            var users = await query.ToListAsync();
            return users.Select(MapUserToUserDTO).ToList();
        }

        public async Task<IReadOnlyList<UserDTO>> GetAllStudentsAsync()
        {
            var studentUsers = await _userManager.Users
                                     .Where(user => user.Role == UserRole.Student)
                                     .ToListAsync();
            return studentUsers.Select(MapUserToUserDTO).ToList();
        }

        public async Task<UserDTO?> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user == null ? null : MapUserToUserDTO(user);
        }

        public async Task<UserDTO> CreateUserAsync(CreateUserDTO dto)
        {
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                throw new ArgumentException($"User with email '{dto.Email}' already exists.");
            }

            var newUser = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                UserName = dto.Email, 
                Role = dto.Role,
                JoinedDate = DateTime.UtcNow,
                Status = "Active", 
                EmailConfirmed = true, 
                Institution = "Helwan University",
                AvatarUrl = "https://upload.wikimedia.org/wikipedia/commons/6/67/User_Avatar.png"
            };

            var result = await _userManager.CreateAsync(newUser, dto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to create user: {errors}");
            }
            return MapUserToUserDTO(newUser);
        }

        public async Task<UserDTO?> UpdateUserAsync(string userId, UpdateUserDTO dto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null; 
            }

            bool needsUpdate = false;

            if (dto.FirstName != null && user.FirstName != dto.FirstName) { user.FirstName = dto.FirstName; needsUpdate = true; }
            if (dto.LastName != null && user.LastName != dto.LastName) { user.LastName = dto.LastName; needsUpdate = true; }

            if (dto.Email != null && user.Email != dto.Email)
            {
                var existingUserWithNewEmail = await _userManager.FindByEmailAsync(dto.Email);
                if (existingUserWithNewEmail != null && existingUserWithNewEmail.Id != userId)
                {
                    throw new ArgumentException($"Another user with email '{dto.Email}' already exists.");
                }
                user.Email = dto.Email;
                user.UserName = dto.Email; 
                needsUpdate = true;
            }

            if (dto.Role.HasValue && user.Role != dto.Role.Value) { user.Role = dto.Role.Value; needsUpdate = true; }

            if (dto.IsActive.HasValue)
            {
                string newStatus = dto.IsActive.Value ? "Active" : "Inactive";
                if (user.Status != newStatus) { user.Status = newStatus; needsUpdate = true; }
            }
            
            if(dto.PhoneNumber != null && user.PhoneNumber != dto.PhoneNumber) { user.PhoneNumber = dto.PhoneNumber; needsUpdate = true; }

            if (needsUpdate)
            {
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"Failed to update user: {errors}");
                }
            }
            return MapUserToUserDTO(user);
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<BulkAddUsersResultDTO> AddUsersFromCsvAsync(UploadUsersCsvDTO uploadDto)
        {
            var result = new BulkAddUsersResultDTO();
            var usersToAdd = new List<User>();
            var parsedRows = new List<ParsedUserCsvRowDTO>();

            try
            {
                using (var reader = new StreamReader(uploadDto.CsvFile.OpenReadStream()))
                {
                    string? line;
                    int rowNumber = 0;
                    bool isFirstLine = true;

                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        rowNumber++;
                        if (isFirstLine) { isFirstLine = false; continue; }
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        var values = line.Split(',');
                        if (values.Length < 4)
                        {
                            result.ErrorMessages.Add($"Row {rowNumber}: Invalid column count. Expected 4 (FirstName,LastName,Email,Password). Found {values.Length}. Line: '{line}'");
                            continue;
                        }
                        parsedRows.Add(new ParsedUserCsvRowDTO
                        {
                            FirstName = values[0].Trim(),
                            LastName = values[1].Trim(),
                            Email = values[2].Trim(),
                            Password = values[3].Trim(),
                            OriginalRowNumber = rowNumber
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessages.Add($"Error processing CSV file: {ex.Message}");
                return result;
            }

            result.TotalRowsAttempted = parsedRows.Count;
            if (!parsedRows.Any() && !result.ErrorMessages.Any(e => e.StartsWith("Error processing CSV file")))
            {
                result.ErrorMessages.Add("CSV file is empty or contains no valid data rows after the header.");
            }


            foreach (var row in parsedRows)
            {
                if (string.IsNullOrWhiteSpace(row.Email) || string.IsNullOrWhiteSpace(row.Password) ||
                    string.IsNullOrWhiteSpace(row.FirstName) || string.IsNullOrWhiteSpace(row.LastName))
                {
                    result.ErrorMessages.Add($"Row {row.OriginalRowNumber}: Missing required fields (FirstName, LastName, Email, Password) for email '{row.Email}'.");
                    continue;
                }

                var existingUser = await _userManager.FindByEmailAsync(row.Email);
                if (existingUser != null)
                {
                    result.ErrorMessages.Add($"Row {row.OriginalRowNumber}: User with email '{row.Email}' already exists.");
                    continue;
                }

                var newUser = new User
                {
                    FirstName = row.FirstName,
                    LastName = row.LastName,
                    Email = row.Email,
                    UserName = row.Email,
                    Role = uploadDto.AssignedRoleForAll,
                    JoinedDate = DateTime.UtcNow,
                    Status = "Active",
                    EmailConfirmed = true,
                    Institution = "Helwan University",
                    AvatarUrl = "https://upload.wikimedia.org/wikipedia/commons/6/67/User_Avatar.png"
                };

                var identityResult = await _userManager.CreateAsync(newUser, row.Password);
                if (identityResult.Succeeded)
                {
                    result.SuccessfullyAddedCount++;
                }
                else
                {
                    var errors = string.Join(", ", identityResult.Errors.Select(e => e.Description));
                    result.ErrorMessages.Add($"Row {row.OriginalRowNumber} (Email: {row.Email}): Failed - {errors}");
                }
            }
            return result;
        }
    }
}