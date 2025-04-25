using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helper.AccountValidations
{
    public static class RegistrationVaildation
    {
        public static void RegisterVaildation(RegisterationDTO registerationDTO)
        {
            if (string.IsNullOrWhiteSpace(registerationDTO.email))
                throw new ArgumentException("Email required");
            if (string.IsNullOrWhiteSpace(registerationDTO.password))
                throw new ArgumentException("password required");
            if (string.IsNullOrWhiteSpace(registerationDTO.FirstName))
                throw new ArgumentException("FirstName required");
            if (string.IsNullOrWhiteSpace(registerationDTO.LastName))
                throw new ArgumentException("LastName required");
            if (string.IsNullOrWhiteSpace(registerationDTO.phonenumber))
                throw new ArgumentException("phonenumber required");
        }
    }
}
