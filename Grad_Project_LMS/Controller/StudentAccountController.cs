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
        private readonly IAccountService _accountService;

        public StudentAccountController
            (
            UserManager<Student> userManager,
            SignInManager<Student> signInManager,
            ITokenService tokenService,
            IConfiguration configuration,
            IEmailService emailService,
            ILogger<StudentAccountController> logger,
            IAccountService accountService

            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _configuration = configuration;
            _emailService = emailService;
            _logger = logger;
            _accountService = accountService;
        }

        [HttpPost("Registeration")]
        public async Task<ActionResult<UserDTO>> Registeration(RegisterationDTO registerationDTO)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var result = await _accountService.Register(registerationDTO);
                return Ok(result);
            }
            catch (ArgumentException aex)
            {
                _logger.LogError(aex.Message);
                return BadRequest(aex.Message);
            }
       
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var result = await _accountService.Login(loginDTO);
                return Ok(result);

            }
            catch(ArgumentException aex)
            {
                _logger.LogError(aex.Message);
                return BadRequest(aex.Message);
            }
            catch (UnauthorizedAccessException iex)
            {
                _logger.LogError(iex.Message);
                return Unauthorized(iex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordDTO forgetPasswordDTO)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var result = await _accountService.ForgetPassword(forgetPasswordDTO);
                return Ok(result);
            }
            catch(ArgumentException aex)
            {
                return BadRequest(aex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
                throw;
            }
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest("invalid modelstate");
                var result = await _accountService.ResetPassword(resetPasswordDTO);
                return Ok(result);
            }
            catch (ArgumentException aex)
            {
                return BadRequest(aex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode (500, ex.Message);
            }
        }
    }
}
