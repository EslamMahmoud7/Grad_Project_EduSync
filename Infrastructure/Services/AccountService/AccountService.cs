using Application.Helper.AccountValidations;
using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Grad_Project_LMS.Controller;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AccountService> _logger;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public AccountService(SignInManager<User> signInManager,
            UserManager<User> userManager,
            ILogger<AccountService> logger,
            ITokenService tokenService,
            IEmailService emailService,
            IConfiguration configuration
            )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _tokenService = tokenService;
            _emailService = emailService;
            _configuration = configuration;
        }


        public async Task<string> ForgetPassword(ForgetPasswordDTO forgetPasswordDTO)
        {
            ForgetPasswordVaildatoin.ValidateForgetPassword(forgetPasswordDTO);

            var Student = await _userManager.FindByEmailAsync(forgetPasswordDTO.Email);
            if (Student == null)
            {
                _logger.LogError("email not exist");
                throw new ArgumentException("Email does not exist");

            }
            var Token = await _userManager.GeneratePasswordResetTokenAsync(Student);
            var resetLink = $"{_configuration["AppUrl"]}/reset-password?email={forgetPasswordDTO.Email}&token={Uri.EscapeDataString(Token)}";

            try
            {
                var subject = "Password Reset Request";
                var body = $"Please reset your password using the following link: <a href='{resetLink}'>Reset Password</a>";
                var toEmail = forgetPasswordDTO.Email;

                await _emailService.SendEmail(toEmail, subject, body);

                _logger.LogInformation($"Password reset link sent to: {toEmail}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception();
            }

            return "reset link sent successfully";
        }
        public async Task<UserDTO> Login(LoginDTO loginDTO)
        {
            LoginVaildation.ValidateLogin(loginDTO);

            var Student = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (Student == null)
            {
                _logger.LogError("email not exist");
                throw new UnauthorizedAccessException("email or password is invalid");
            }
            var result = await _signInManager.CheckPasswordSignInAsync(Student, loginDTO.Password, false);

            if (!result.Succeeded)
            {
                _logger.LogError("email or password incorrect");
                throw new UnauthorizedAccessException("email or password incorrect");

            }
            return new UserDTO()
            {
                FirstName = Student.FirstName ?? "unknown fname",
                LastName = Student.LastName ?? "unknown lname",
                Email = loginDTO.Email,
                Token = await _tokenService.GenerateJWTToken(Student)
            };
        }
        public async Task<UserDTO> Register(RegisterationDTO registerationDTO)
        {
            RegistrationVaildation.RegisterVaildation(registerationDTO);

            if (await _userManager.FindByEmailAsync(registerationDTO.Email) != null)
                _logger.LogError("email already exist");

            var Student = new User()
            {
                FirstName = registerationDTO.FirstName,
                LastName = registerationDTO.LastName,
                Email = registerationDTO.Email,
                UserName = registerationDTO.Email.Split('@')[0]
            };

            var result = await _userManager.CreateAsync(Student, registerationDTO.Password);
            if (!result.Succeeded) result.Errors.Select(error => error.Description);

            return new UserDTO()
            {
                FirstName = registerationDTO.FirstName,
                LastName = registerationDTO.LastName,
                Email = registerationDTO.Email,
                Password = registerationDTO.Password,
                Token = await _tokenService.GenerateJWTToken(Student)
            };
        }
        public async Task<string> ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            ResetPasswordVaildation.ValidateResetPassword(resetPasswordDTO);
            var Student = await _userManager.FindByEmailAsync(resetPasswordDTO.Email);
            if (Student == null)
            {
                _logger.LogError("student not exist");
                throw new ArgumentException("student is null");
            }
            var DecodedToken = WebUtility.UrlDecode(resetPasswordDTO.Token);
            var Result = await _userManager.ResetPasswordAsync(Student, DecodedToken, resetPasswordDTO.NewPassword);
            if (!Result.Succeeded)
            {
                throw new ArgumentException();
            }
            return "Password has been reset successfully";
        }
    }
}
