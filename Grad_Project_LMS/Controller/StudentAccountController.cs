using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Grad_Project_LMS.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentAccountController : ControllerBase
    {
        private readonly UserManager<Student> _userManager;
        private readonly SignInManager<Student> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly ILogger<StudentAccountController> _logger;

        public StudentAccountController
            (
            UserManager<Student> userManager,
            SignInManager<Student> signInManager,
            ITokenService tokenService,
            IConfiguration configuration,
            IEmailService emailService,
            ILogger<StudentAccountController> logger

            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _configuration = configuration;
            _emailService = emailService;
            _logger = logger;
        }

        [HttpPost("Registeration")]
        public async Task<ActionResult<UserDTO>> Registeration(RegisterationDTO registerationDTO)
        {
            //Helper.Vaildations.VaildateRegistration(registerationDTO);


            if (await _userManager.FindByEmailAsync(registerationDTO.email) != null)
                return BadRequest($"email already exists");

            if (!ModelState.IsValid)
                return BadRequest("validation error");

            var Student = new Student()
            {
                Email = registerationDTO.email,
                FirstName = registerationDTO.FirstName,
                LastName = registerationDTO.LastName,
                FirstLogin = DateTime.Now,
                UserName = registerationDTO.email.Split("@")[0]

            };

            var result = await _userManager.CreateAsync(Student, registerationDTO.password);

            if (!result.Succeeded)
            {
                var errros = result.Errors.Select(error => error.Description);
                return BadRequest(errros);
            }

            try
            {
                return Ok(new UserDTO
                {
                    FirstName = registerationDTO.FirstName,
                    LastName = registerationDTO.LastName,
                    Email = registerationDTO.email,
                    Password = registerationDTO.password,
                    Token = await _tokenService.GenerateJWTToken(Student)
                });
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var Student = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (Student == null) return BadRequest("email ");

            var Result = await _signInManager.CheckPasswordSignInAsync(Student, loginDTO.Password, false);
            if (!Result.Succeeded) return BadRequest("email or password is incorrect");

            return Ok(new UserDTO()
            {
                FirstName = Student.FirstName ?? "unknown",
                LastName = Student.LastName ?? "unknown",
                Email = loginDTO.Email,
                Password = loginDTO.Password,
                Token = await _tokenService.GenerateJWTToken(Student)
            });
        }

        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordDTO forgetPasswordDTO)
        {
            var Student = await _userManager.FindByEmailAsync(forgetPasswordDTO.Email);
            if (Student == null) return BadRequest("this email does not exist");

            var Token = await _userManager.GeneratePasswordResetTokenAsync(Student);
            var resetLink = $"{_configuration["AppUrl"]}/reset-password?email={forgetPasswordDTO.Email}&token={Uri.EscapeDataString(Token)}";

            try
            {
                var subject = "Password Reset Request";
                var body = $"Please reset your password using the following link: <a href='{resetLink}'>Reset Password</a>";
                var toEmail = forgetPasswordDTO.Email;

                // Send the email (replace with your own email sending logic)
                await _emailService.SendEmail(toEmail, subject, body);

                _logger.LogInformation($"Password reset link sent to: {toEmail}");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Error sending email. Please try again later.");
            }
            return Ok("reset link sent for your rmail");
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {

            var student = await _userManager.FindByEmailAsync(resetPasswordDTO.Email);
            if (student is null) return BadRequest("This email does not exist");

            var decodedToken = WebUtility.UrlDecode(resetPasswordDTO.Token);

            var result = await _userManager.ResetPasswordAsync(student, decodedToken, resetPasswordDTO.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(error => error.Description).ToList();
                _logger.LogWarning("ResetPassword errors: {Errors}", string.Join(", ", errors));
                return BadRequest(errors);
            }
            return Ok("Password has been reset successfully");
        }
    }
}
