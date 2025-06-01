using Domain.DTOs;
using Domain.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Grad_Project_LMS.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminDashboardController : ControllerBase
    {
        private readonly IAdminDashboardService _adminDashboardService;

        public AdminDashboardController(IAdminDashboardService adminDashboardService)
        {
            _adminDashboardService = adminDashboardService;
        }

        [HttpGet("counts")]
        public async Task<ActionResult<AdminDashboardCountsDTO>> GetCounts()
        {
            try
            {
                var counts = await _adminDashboardService.GetDashboardCountsAsync();
                return Ok(counts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An internal server error occurred while retrieving dashboard counts.");
            }
        }
    }
}