using Domain.DTOs;
using Domain.Interfaces.IServices;
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
                return StatusCode(500, "An internal server error occurred while retrieving students.");
            }
        }
    }
}