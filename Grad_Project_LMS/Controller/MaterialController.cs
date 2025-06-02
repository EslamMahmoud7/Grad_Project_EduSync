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
    public class MaterialController : ControllerBase
    {
        private readonly IMaterialService _materialService;

        public MaterialController(IMaterialService materialService)
        {
            _materialService = materialService;
        }

        [HttpPost]
        public async Task<ActionResult<MaterialDTO>> AddMaterial([FromBody] CreateMaterialDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (string.IsNullOrEmpty(dto.UploadingInstructorId))
                return BadRequest("UploadingInstructorId is required.");

            try
            {
                var material = await _materialService.AddMaterialAsync(dto);
                return CreatedAtAction(nameof(GetMaterialById), new { materialId = material.Id }, material);
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
            catch (InvalidOperationException ex) { return Conflict(ex.Message); }
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }

        [HttpGet("{materialId}")]
        public async Task<ActionResult<MaterialDTO>> GetMaterialById(string materialId)
        {
            try
            {
                var material = await _materialService.GetMaterialByIdAsync(materialId);
                if (material == null) return NotFound($"Material with ID '{materialId}' not found.");
                return Ok(material);
            }
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }

        [HttpGet("group/{groupId}")]
        public async Task<ActionResult<IReadOnlyList<MaterialDTO>>> GetMaterialsByGroup(string groupId)
        {
            if (string.IsNullOrEmpty(groupId)) return BadRequest("Group ID is required.");
            try
            {
                var materials = await _materialService.GetMaterialsByGroupIdAsync(groupId);
                return Ok(materials);
            }
            catch (ArgumentException ex) { return NotFound(ex.Message); }
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }

        [HttpPut("{materialId}")]
        public async Task<ActionResult<MaterialDTO>> UpdateMaterial(string materialId, [FromBody] UpdateMaterialDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (string.IsNullOrEmpty(dto.UpdatingInstructorId))
                return BadRequest("UpdatingInstructorId is required.");
            try
            {
                var updatedMaterial = await _materialService.UpdateMaterialAsync(materialId, dto);
                return Ok(updatedMaterial);
            }
            catch (ArgumentException ex) { return NotFound(ex.Message); } 
            catch (InvalidOperationException ex) { return Conflict(ex.Message); } 
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }

        [HttpDelete("{materialId}")]
        public async Task<IActionResult> DeleteMaterial(string materialId, [FromQuery] string requestingUserId)
        {
            if (string.IsNullOrEmpty(materialId) || string.IsNullOrEmpty(requestingUserId))
                return BadRequest("Material ID and Requesting User ID are required.");
            try
            {
                var success = await _materialService.DeleteMaterialAsync(materialId, requestingUserId);
                if (!success) return NotFound($"Material with ID '{materialId}' not found or delete failed.");
                return NoContent();
            }
            catch (ArgumentException ex) { return NotFound(ex.Message); }
            catch (InvalidOperationException ex) { return Conflict(ex.Message); }
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }
    }
}