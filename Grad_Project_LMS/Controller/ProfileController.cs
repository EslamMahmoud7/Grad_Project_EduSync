using Domain.DTOs;
using Domain.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Grad_Project_LMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<ProfileDTO>> GetProfile(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("userId is required");

            try
            {
                var dto = await _profileService.GetProfileById(userId);
                return Ok(dto);
            }
            catch (ArgumentException)
            {
                return NotFound($"No profile found for user {userId}");
            }
            catch
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
