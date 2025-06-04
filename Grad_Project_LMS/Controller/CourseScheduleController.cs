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

    public CourseScheduleController(ICourseScheduleService courseScheduleService)
    {
        _svc = courseScheduleService;
    }

    [HttpGet("student/{studentId}")]
    public async Task<ActionResult<IReadOnlyList<CourseScheduleDTO>>> GetStudentSchedule(string studentId)
    {
        if (string.IsNullOrWhiteSpace(studentId))
        {
            return BadRequest("Student ID cannot be empty.");
        }
        var schedule = await _svc.GetForStudentAsync(studentId);
        return Ok(schedule);
    }
    [HttpGet("instructor/{instructorId}")]
    public async Task<ActionResult<IReadOnlyList<CourseScheduleDTO>>> GetInstructorSchedule(string instructorId)
    {
        if (string.IsNullOrWhiteSpace(instructorId))
        {
            return BadRequest("Instructor ID cannot be empty.");
        }
        var schedule = await _svc.GetForInstructorAsync(instructorId);
        return Ok(schedule);
    }
}