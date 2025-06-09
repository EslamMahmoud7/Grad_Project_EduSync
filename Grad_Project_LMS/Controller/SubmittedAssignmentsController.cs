using Domain.DTOs;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Grad_Project_LMS.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubmittedAssignmentsController : ControllerBase
    {
        private readonly ISubmittedAssignmentService _submittedAssignmentService;

        public SubmittedAssignmentsController(ISubmittedAssignmentService submittedAssignmentService)
        {
            _submittedAssignmentService = submittedAssignmentService;
        }

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitAssignment([FromBody] SubmitAssignmentDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var submittedAssignment = await _submittedAssignmentService.SubmitAssignmentAsync(dto);
                return CreatedAtAction(nameof(GetSubmittedAssignmentById), new { id = submittedAssignment.Id }, submittedAssignment);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
        }

        [HttpGet("instructor/{instructorId}")]
        public async Task<IActionResult> GetInstructorSubmittedAssignments(string instructorId)
        {
            try
            {
                var submittedAssignments = await _submittedAssignmentService.GetInstructorSubmittedAssignmentsAsync(instructorId);
                return Ok(submittedAssignments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}/grade")]
        public async Task<IActionResult> GradeSubmittedAssignment(string id, [FromBody] GradeSubmittedAssignmentDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var gradedAssignment = await _submittedAssignmentService.GradeSubmittedAssignmentAsync(id, dto);
                return Ok(gradedAssignment);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetStudentSubmittedAssignments(string studentId, [FromQuery] string assignmentId = null)
        {
            try
            {
                var submittedAssignments = await _submittedAssignmentService.GetStudentSubmittedAssignmentsAsync(studentId, assignmentId);
                return Ok(submittedAssignments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubmission(string id, [FromBody] UpdateSubmissionDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var updatedAssignment = await _submittedAssignmentService.UpdateSubmissionAsync(id, dto);
                return Ok(updatedAssignment);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubmission(string id)
        {
            try
            {
                await _submittedAssignmentService.DeleteSubmissionAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubmittedAssignmentById(string id)
        {
            try
            {
                var submittedAssignment = await _submittedAssignmentService.GetSubmittedAssignmentByIdAsync(id);
                return Ok(submittedAssignment);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
