using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helper.AccountValidations
{
    public static class ResetPasswordVaildation
    {
        public static void ValidateResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            if (resetPasswordDTO.Email == null)
                throw new ArgumentException("email is required");
            if (resetPasswordDTO.NewPassword == null)
                throw new ArgumentException("new password is required");
            if (resetPasswordDTO.Token == null)
                throw new ArgumentException("Token is required");
        }
    }
}
