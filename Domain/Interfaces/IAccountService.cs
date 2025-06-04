using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IAccountService
    {
        public Task<UserDTO> Register(RegisterationDTO registerationDTO);
        public Task<UserDTO> Login(LoginDTO loginDTO);
        public Task<string> ForgetPassword(ForgetPasswordDTO forgetPasswordDTO);
        public Task<string> ResetPassword(ResetPasswordDTO resetPasswordDTO);
    }
}
