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
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpPost]
        public async Task<ActionResult<GroupDTO>> AddGroup([FromBody] CreateGroupDTO dto)
        {
            if (!ModelState.IsValid)
            {   
                return BadRequest(ModelState);
            }
            try
            {
                var group = await _groupService.AddGroupAsync(dto);
                return Ok(group);
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
        public async Task<ActionResult<GroupDTO>> GetGroup(string id)
        {
            try
            {
                var group = await _groupService.GetGroupByIdAsync(id);
                return Ok(group);
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
        public async Task<ActionResult<IReadOnlyList<GroupDTO>>> GetAllGroups()
        {
            try
            {
                var groups = await _groupService.GetAllGroupsAsync();
                return Ok(groups);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("course/{courseId}")]
        public async Task<ActionResult<IReadOnlyList<GroupDTO>>> GetGroupsByCourseId(string courseId)
        {
            try
            {
                var groups = await _groupService.GetGroupsByCourseIdAsync(courseId);
                return Ok(groups);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("instructor/{instructorId}")]
        public async Task<ActionResult<IReadOnlyList<GroupDTO>>> GetGroupsByInstructorId(string instructorId)
        {
            try
            {
                var groups = await _groupService.GetGroupsByInstructorIdAsync(instructorId);
                return Ok(groups);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<GroupDTO>> UpdateGroup(string id, [FromBody] UpdateGroupDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var updatedGroup = await _groupService.UpdateGroupAsync(id, dto);
                return Ok(updatedGroup);
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
        public async Task<IActionResult> DeleteGroup(string id)
        {
            try
            {
                await _groupService.DeleteGroupAsync(id);
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