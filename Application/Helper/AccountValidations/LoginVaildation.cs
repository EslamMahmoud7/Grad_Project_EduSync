using Domain.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helper.AccountValidations
{
    public static class LoginVaildation
    {
        public static void ValidateLogin(LoginDTO loginDTO)
        {
            if (string.IsNullOrWhiteSpace(loginDTO.Email))
                throw new ArgumentException("email is null");
            if (string.IsNullOrWhiteSpace(loginDTO.Password))
                throw new ArgumentException("password is null");
        }
    }
}
