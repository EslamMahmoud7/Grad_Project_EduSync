using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profileService;

    public ProfileController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    // GET /api/profile
    [HttpGet("{userId}")]
    public async Task<ActionResult<ProfileDTO>> GetProfile(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return Unauthorized();

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
