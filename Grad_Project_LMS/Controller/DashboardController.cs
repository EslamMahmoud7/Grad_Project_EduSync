using Domain.DTOs;
using Domain.Interfaces.IServices;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _svc;
    public DashboardController(IDashboardService svc) => _svc = svc;

    [HttpGet("{userId}")]
    public async Task<ActionResult<DashboardDTO>> Get(string userId)
    {
        try
        {
            var dto = await _svc.GetForStudentAsync(userId);
            return Ok(dto);
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
    }
}
