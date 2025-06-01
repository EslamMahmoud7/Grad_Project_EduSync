using Domain.DTOs;
using Domain.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;


namespace Grad_Project_LMS.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class InstructorDashboardController : ControllerBase
    {
        private readonly IInstructorDashboardService _instructorDashboardService;

        public InstructorDashboardController(IInstructorDashboardService instructorDashboardService)
        {
            _instructorDashboardService = instructorDashboardService;
        }

        [HttpGet("{instructorId}/counts")]
        public async Task<ActionResult<InstructorDashboardCountsDTO>> GetCounts(string instructorId)
        {
            if (string.IsNullOrEmpty(instructorId))
            {
                return BadRequest("Instructor ID is required.");
            }

            try
            {
                var counts = await _instructorDashboardService.GetDashboardCountsAsync(instructorId);
                return Ok(counts);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An internal server error occurred while retrieving instructor dashboard counts.");
            }
        }
    }
}