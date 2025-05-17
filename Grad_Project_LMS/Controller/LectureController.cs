using Domain.DTOs;
using Domain.Interfaces.IServices;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Grad_Project_LMS.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class LectureController : ControllerBase
    {
        private readonly ILectureService _lectureService;

        public LectureController(ILectureService lectureService)
        {
            _lectureService = lectureService;
        }
        [HttpPost]
        public async Task<ActionResult<LectureDTO>> AddLecture([FromBody] AddLectureDTO lectureDTO)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest("model state is invalid");
                var result = await _lectureService.Add(lectureDTO);
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

        [HttpPut("UpdateLecture")]
        public async Task<ActionResult<LectureDTO>> UpdateLecture([FromBody] AddLectureDTO lectureDTO)
        {
            try
            {
                var result = await _lectureService.Update(lectureDTO);
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
        public async Task<ActionResult> DeleteLecture(int id)
        {
            try
            {
                await _lectureService.Delete(id);
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

        [HttpGet("GetAllLectures")]
        public async Task<ActionResult<IReadOnlyList<LectureDTO>>> GetAllLectures()
        {
            try
            {
                var AllLectures = await _lectureService.GetAll();
                return Ok(AllLectures);
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

        [HttpGet("GetAllPaginatedLectures")]
        public async Task<ActionResult<PaginatedResultDTO<LectureDTO>>> GetAllPaginatedLectures([FromQuery] int PageNumber = 1, [FromQuery] int PageSize = 3)
        {
            try
            {
                var PaginatedLectures = await _lectureService.GetAllPaginated(PageNumber, PageSize);
                return Ok(PaginatedLectures);
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
        public async Task<ActionResult<LectureDTO>> GetById(int id)
        {
            try
            {
                var Courese = await _lectureService.Get(id);
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
