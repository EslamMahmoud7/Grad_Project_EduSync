using Domain.DTOs;
using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces.IServices
{
    public interface IUserService
    {        
        Task<IReadOnlyList<UserDTO>> GetAllUsersAsync(UserRole? roleFilter = null);

        Task<IReadOnlyList<UserDTO>> GetAllStudentsAsync();

        Task<UserDTO?> GetUserByIdAsync(string userId); 
        Task<UserDTO> CreateUserAsync(CreateUserDTO dto);
        Task<UserDTO?> UpdateUserAsync(string userId, UpdateUserDTO dto);
        Task<bool> DeleteUserAsync(string userId);
        Task<BulkAddUsersResultDTO> AddUsersFromCsvAsync(UploadUsersCsvDTO uploadDto);
    }
}