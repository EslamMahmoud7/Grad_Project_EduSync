using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helper.AccountValidations
{
    public static class ForgetPasswordVaildatoin
    {
        public static void ValidateForgetPassword(ForgetPasswordDTO forgetPasswordDTO)
        {
            if (forgetPasswordDTO.Email == null)
                throw new ArgumentException("email is null");
        }
    }
}
