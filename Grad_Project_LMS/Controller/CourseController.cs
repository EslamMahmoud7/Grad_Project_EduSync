using Domain.DTOs;
using Domain.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Grad_Project_LMS.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpPost]
        public async Task<ActionResult<CourseDto>> AddCourse([FromBody] CourseDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _courseService.Add(dto);
            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult<CourseDto>> UpdateCourse([FromBody] CourseDto dto, string CourseId)
        {
            var result = await _courseService.Update(dto, CourseId);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(string id)
        {
            await _courseService.Delete(id);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDto>> GetCourse(string id)
        {
            var result = await _courseService.Get(id);
            return Ok(result);
        }

        [HttpGet("all")]
        public async Task<ActionResult<IReadOnlyList<CourseDto>>> GetAllCourses()
        {
            var result = await _courseService.GetAll();
            return Ok(result);
        }

        [HttpGet("mine/{studentId}")]
        public async Task<ActionResult<IReadOnlyList<CourseDto>>> GetMyCourses(string studentId)
        {
            if (string.IsNullOrWhiteSpace(studentId)) return Unauthorized();

            var result = await _courseService.GetForStudent(studentId);
            return Ok(result);
        }

        [HttpPost("assign-course")]
        public async Task<IActionResult> AssignCourse([FromBody] EnrollmentDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _courseService.AssignCourseAsync(dto.StudentId, dto.CourseId);
                return Ok(new { message = "Student enrolled in course successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
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
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while assigning student to group.");
            }
        }
    }
}