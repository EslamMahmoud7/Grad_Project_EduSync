using Domain.DTOs;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AssignmentController : ControllerBase
{
    private readonly IAssignmentService _svc;
    public AssignmentController(IAssignmentService svc) => _svc = svc;

    [HttpPost]
    public async Task<ActionResult<AssignmentDTO>> Post(CreateAssignmentDTO dto)
        => Ok(await _svc.AddAssignmentAsync(dto));

    [HttpGet("mine/{studentId}")]
    public async Task<ActionResult<IReadOnlyList<AssignmentDTO>>> Mine(string studentId)
        => Ok(await _svc.GetForStudentAsync(studentId));
}
