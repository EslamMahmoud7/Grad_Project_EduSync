using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Grad_Project_LMS.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<UserDTO>>> GetAllUsers([FromQuery] UserRole? roleFilter)
        {
            try
            {
                var users = await _userService.GetAllUsersAsync(roleFilter);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("students")]
        public async Task<ActionResult<IReadOnlyList<UserDTO>>> GetAllStudents()
        {
            try
            {
                var students = await _userService.GetAllStudentsAsync();
                return Ok(students);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("{userId}")]
        public async Task<ActionResult<UserDTO>> GetUser(string userId)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null) return NotFound($"User with ID '{userId}' not found.");
                return Ok(user);
            }
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }


        [HttpPost]
        public async Task<ActionResult<UserDTO>> CreateUser([FromBody] CreateUserDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var newUser = await _userService.CreateUserAsync(dto);
                return CreatedAtAction(nameof(GetUser), new { userId = newUser.Id }, newUser);
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
            catch (InvalidOperationException ex) { return Conflict(ex.Message); } 
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }

        [HttpPut("{userId}")]
        public async Task<ActionResult<UserDTO>> UpdateUser(string userId, [FromBody] UpdateUserDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var updatedUser = await _userService.UpdateUserAsync(userId, dto);
                if (updatedUser == null) return NotFound($"User with ID '{userId}' not found.");
                return Ok(updatedUser);
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
            catch (InvalidOperationException ex) { return Conflict(ex.Message); }
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            try
            {
                var success = await _userService.DeleteUserAsync(userId);
                if (!success) return NotFound($"User with ID '{userId}' not found.");
                return NoContent();
            }
            catch (InvalidOperationException ex) { return Conflict(ex.Message); } 
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }

        [HttpPost("bulk-upload-csv")]
        public async Task<ActionResult<BulkAddUsersResultDTO>> AddUsersFromCsv([FromForm] UploadUsersCsvDTO uploadDto)
        {
            if (uploadDto.CsvFile == null || uploadDto.CsvFile.Length == 0)
            {
                return BadRequest("A CSV file is required.");
            }
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _userService.AddUsersFromCsvAsync(uploadDto);
                if (result.ErrorMessages.Any() && result.SuccessfullyAddedCount == 0)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BulkAddUsersResultDTO { ErrorMessages = new List<string> { $"Internal server error: {ex.Message}" } });
            }
        }
    }
}