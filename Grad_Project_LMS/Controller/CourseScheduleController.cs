using Domain.DTOs;
using Domain.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class CourseScheduleController : ControllerBase
{
    private readonly ICourseScheduleService _svc;
    public CourseScheduleController(ICourseScheduleService svc) => _svc = svc;

    [HttpGet("mine/{studentId}")]
    public async Task<ActionResult<IReadOnlyList<CourseScheduleDTO>>> Mine(string studentId)
        => Ok(await _svc.GetForStudentAsync(studentId));
}
