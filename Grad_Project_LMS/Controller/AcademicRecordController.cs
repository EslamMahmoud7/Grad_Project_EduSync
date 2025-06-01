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
    public class AcademicRecordController : ControllerBase
    {
        private readonly IAcademicRecordService _academicRecordService;

        public AcademicRecordController(IAcademicRecordService academicRecordService)
        {
            _academicRecordService = academicRecordService;
        }

        [HttpPost("bulk-upload-csv")]
        public async Task<ActionResult<BulkAddAcademicRecordsResultDTO>> AddAcademicRecordsFromCsv([FromForm] UploadAcademicRecordsCsvDTO uploadDto)
        {
            if (uploadDto.CsvFile == null || uploadDto.CsvFile.Length == 0)
            {
                return BadRequest("A CSV file is required.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _academicRecordService.AddAcademicRecordsFromCsvAsync(uploadDto);
                if (result.ErrorMessages.Any() && result.SuccessfullyAddedCount == 0)
                {
                    return BadRequest(result);
                }
                if (result.ErrorMessages.Any())
                {
                    return Ok(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BulkAddAcademicRecordsResultDTO { ErrorMessages = new List<string> { $"Internal server error: {ex.Message}" } });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AcademicRecordDTO>> GetAcademicRecord(string id)
        {
            try
            {
                var record = await _academicRecordService.GetAcademicRecordByIdAsync(id);
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