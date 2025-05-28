using Domain.DTOs;
using Domain.Interfaces.IServices;
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
        public async Task<ActionResult<CourseDto>> UpdateCourse([FromBody] CourseDto dto)
        {
            var result = await _courseService.Update(dto);
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
    }
}
