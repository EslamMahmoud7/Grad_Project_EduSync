// Grad_Project_LMS/Controller/AssignmentController.cs
using Domain.DTOs;
using Domain.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

[ApiController]
[Route("api/[controller]")]
public class AssignmentController : ControllerBase
{
    private readonly IAssignmentService _svc;
    public AssignmentController(IAssignmentService svc) => _svc = svc;

    [HttpPost]
    public async Task<ActionResult<AssignmentDTO>> AddAssignment([FromBody] CreateAssignmentDTO dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            return Ok(await _svc.AddAssignmentAsync(dto));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while adding the assignment: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AssignmentDTO>> GetAssignment(string id)
    {
        try
        {
            var assignment = await _svc.GetAssignmentByIdAsync(id);
            return Ok(assignment);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while fetching the assignment: {ex.Message}");
        }
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AssignmentDTO>>> GetAllAssignments()
    {
        try
        {
            var assignments = await _svc.GetAllAssignmentsAsync();
            return Ok(assignments);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while fetching all assignments: {ex.Message}");
        }
    }

    [HttpGet("mine/{studentId}")]
    public async Task<ActionResult<IReadOnlyList<AssignmentDTO>>> GetMyAssignments(string studentId)
    {
        if (string.IsNullOrWhiteSpace(studentId)) return BadRequest("Student ID cannot be empty.");
        try
        {
            return Ok(await _svc.GetForStudentAsync(studentId));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while fetching student's assignments: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<AssignmentDTO>> UpdateAssignment(string id, [FromBody] UpdateAssignmentDTO dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var updatedAssignment = await _svc.UpdateAssignmentAsync(id, dto);
            return Ok(updatedAssignment);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while updating the assignment: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAssignment(string id)
    {
        try
        {
            await _svc.DeleteAssignmentAsync(id);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while deleting the assignment: {ex.Message}");
        }
    }
}