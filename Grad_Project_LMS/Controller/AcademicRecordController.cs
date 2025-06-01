using Domain.DTOs;
using Domain.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
namespace Grad_Project_LMS.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AcademicRecordController : ControllerBase
    {
        private readonly IAcademicRecordService _academicRecordService;

        public AcademicRecordController(IAcademicRecordService academicRecordService)
        {
            _academicRecordService = academicRecordService;
        }

        [HttpPost]
        public async Task<ActionResult<AcademicRecordDTO>> AddAcademicRecord([FromBody] CreateAcademicRecordDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var record = await _academicRecordService.AddAcademicRecordAsync(dto);
                return CreatedAtAction(nameof(GetAcademicRecord), new { id = record.Id }, record);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AcademicRecordDTO>> GetAcademicRecord(string id)
        {
            try
            {
                var record = await _academicRecordService.GetAcademicRecordByIdAsync(id);
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (User.IsInRole(Domain.Entities.UserRole.Student.ToString()) && record.StudentId != userId)
                {
                    return Forbid();
                }
                return Ok(record);
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

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<AcademicRecordDTO>>> GetAllAcademicRecords()
        {
            try
            {
                var records = await _academicRecordService.GetAllAcademicRecordsAsync();
                return Ok(records);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<IReadOnlyList<AcademicRecordDTO>>> GetAcademicRecordsByStudentId(string studentId)
        {
            var currentUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (User.IsInRole(Domain.Entities.UserRole.Student.ToString()) && currentUserId != studentId)
            {
                return Forbid();
            }

            try
            {
                var records = await _academicRecordService.GetAcademicRecordsByStudentIdAsync(studentId);
                return Ok(records);
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

        [HttpGet("group/{groupId}")]
        public async Task<ActionResult<IReadOnlyList<AcademicRecordDTO>>> GetAcademicRecordsByGroupId(string groupId)
        {

            try
            {
                var records = await _academicRecordService.GetAcademicRecordsByGroupIdAsync(groupId);
                return Ok(records);
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

        [HttpPut("{id}")]
        public async Task<ActionResult<AcademicRecordDTO>> UpdateAcademicRecord(string id, [FromBody] UpdateAcademicRecordDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var updatedRecord = await _academicRecordService.UpdateAcademicRecordAsync(id, dto);
                return Ok(updatedRecord);
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

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAcademicRecord(string id)
        {
            try
            {
                await _academicRecordService.DeleteAcademicRecordAsync(id);
                return NoContent();
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