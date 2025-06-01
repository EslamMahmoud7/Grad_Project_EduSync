using Domain.DTOs;
using Grad_Project_LMS.Controller;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces.IServices
{
    public interface IUserService
    {
        Task<IReadOnlyList<UserDTO>> GetAllStudentsAsync();
    }
}