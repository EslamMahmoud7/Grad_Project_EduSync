using Domain.DTOs;
using Domain.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace Grad_Project_LMS.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpPost]
        public async Task<ActionResult<CourseDTO>> AddCourse([FromBody]CourseDTO courseDTO)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest("model state is invalid") ;
                var result = await _courseService.Add(courseDTO);
                return Ok(result);
            }
            catch (ArgumentException aex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPut("UpdateCourse")]
        public async Task<ActionResult<CourseDTO>> UpdateCourse([FromBody]CourseDTO courseDTO)
        {
            try
            {
                var result = await _courseService.Update(courseDTO);
                return Ok(result);
            }
            catch (ArgumentException aex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteCourse(int id)
        {
            try
            {
                await _courseService.Delete(id);
                return NoContent();
            }
            catch (ArgumentException aex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet("GetAllCourses")]
        public async Task<ActionResult<IReadOnlyList<CourseDTO>>> GetAllCourses()
        {
            try
            {
                var AllCourses = await _courseService.GetAll();
                return Ok(AllCourses);
            }
            catch (ArgumentException aex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetAllPaginatedCourses")]
        public async Task<ActionResult<PaginatedResultDTO<CourseDTO>>> GetAllPaginatedCourses([FromQuery] int PageNumber = 1, [FromQuery] int PageSize = 3)
        {
            try
            {
                var PaginatedCourses = await _courseService.GetAllPaginated(PageNumber, PageSize);
                return Ok(PaginatedCourses);
            }
            catch (ArgumentException aex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

       [HttpGet("{id:int}")]
        public async Task<ActionResult<CourseDTO>> GetById(int id)
        {
            try
            {
                var Courese = await _courseService.Get(id);
                return Ok(Courese);
            }
            catch (ArgumentException aex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
