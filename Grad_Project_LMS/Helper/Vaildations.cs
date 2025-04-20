using Grad_Project_LMS.DTOs;
using Grad_Project_LMS.Models;
using Microsoft.AspNetCore.Identity;

namespace Grad_Project_LMS.Helper
{
    public static class Vaildations
    {


        public static void VaildateRegistration(RegisterationDTO registerationDTO)
        {
            if (string.IsNullOrWhiteSpace(registerationDTO.email)) throw new ArgumentException("email is null");
            if (string.IsNullOrWhiteSpace(registerationDTO.FirstName)) throw new ArgumentException("fname is null");
            if (string.IsNullOrWhiteSpace(registerationDTO.LastName)) throw new ArgumentException("lname is null");
        }

    }
}
