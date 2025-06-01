using Domain.DTOs;
using Domain.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Grad_Project_LMS.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly IGroupService _groupService;

        public CourseController(ICourseService courseService, IGroupService groupService)
        {
            _courseService = courseService;
            _groupService = groupService;
        }

        [HttpPost]
        public async Task<ActionResult<CourseDto>> AddCourse([FromBody] CreateCourseDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var result = await _courseService.Add(dto);
                return CreatedAtAction(nameof(GetCourse), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error adding course: {ex.Message}");
            }
        }

        [HttpPut("{CourseId}")]
        public async Task<ActionResult<CourseDto>> UpdateCourse(string CourseId, [FromBody] UpdateCourseDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var result = await _courseService.Update(dto, CourseId);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating course: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(string id)
        {
            try
            {
                await _courseService.Delete(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting course: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDto>> GetCourse(string id)
        {
            try
            {
                var result = await _courseService.Get(id);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching course: {ex.Message}");
            }
        }

        [HttpGet("all")]
        public async Task<ActionResult<IReadOnlyList<CourseDto>>> GetAllCourses()
        {
            try
            {
                var result = await _courseService.GetAll();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching all courses: {ex.Message}");
            }
        }

        [HttpPost("assign-to-group")]
        public async Task<IActionResult> AssignStudentToGroup([FromBody] GroupEnrollmentDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _courseService.AssignStudentToGroupAsync(dto);
                return Ok(new { message = "Student assigned to group successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while assigning student to group: {ex.Message}");
            }
        }

        [HttpGet("my-courses-via-groups/{studentId}")]
        public async Task<ActionResult<IReadOnlyList<CourseDto>>> GetMyCoursesViaGroups(string studentId)
        {
            if (string.IsNullOrWhiteSpace(studentId)) return Unauthorized();

            try
            {
                var enrolledGroups = await _groupService.GetGroupsByStudentIdAsync(studentId);

                var distinctCourses = enrolledGroups
                                        .Select(g => new CourseDto
                                        {
                                            Id = g.CourseId,
                                            Title = g.CourseTitle,
                                            Description = g.CourseDescription,
                                            Credits = g.CourseCredits,
                                            ResourceLink = g.CourseResourceLink,
                                            Level = g.CourseLevel
                                        })
                                        .DistinctBy(c => c.Id)
                                        .ToList();
                return Ok(distinctCourses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching courses for student via groups: {ex.Message}");
            }
        }

    [HttpPost("assign-students-to-group-bulk")]
        [Authorize(Roles = "Admin, Instructor")]
        public async Task<ActionResult<BulkEnrollmentResultDTO>> AssignStudentsToGroupBulk([FromBody] BulkGroupEnrollmentDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _courseService.AssignStudentsToGroupBulkAsync(dto);

                if (result.Errors.Any())
                {
                    if (result.Errors.ContainsKey("_global"))
                    {
                        return BadRequest(result.Errors["_global"]);
                    }
                    return Ok(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred during bulk student assignment: {ex.Message}");
            }
        }
    }
}