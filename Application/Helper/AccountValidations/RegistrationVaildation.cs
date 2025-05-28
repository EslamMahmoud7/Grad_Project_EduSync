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
            if (string.IsNullOrWhiteSpace(registerationDTO.Email))
                throw new ArgumentException("Email required");
            if (string.IsNullOrWhiteSpace(registerationDTO.Password))
                throw new ArgumentException("password required");
            if (string.IsNullOrWhiteSpace(registerationDTO.FirstName))
                throw new ArgumentException("FirstName required");
            if (string.IsNullOrWhiteSpace(registerationDTO.LastName))
                throw new ArgumentException("FirstName required");
            if (string.IsNullOrWhiteSpace(registerationDTO.PhoneNumber))
                throw new ArgumentException("phonenumber required");
        }
    }
}
