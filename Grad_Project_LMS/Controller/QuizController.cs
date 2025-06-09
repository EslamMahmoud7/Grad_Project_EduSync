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
    public class QuizController : ControllerBase
    {
        private readonly IQuizService _quizService;

        public QuizController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        [HttpPost]
        public async Task<ActionResult<QuizDTO>> CreateQuiz([FromBody] CreateQuizDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var quiz = await _quizService.CreateQuizAsync(dto, dto.RequestingInstructorId);
                return CreatedAtAction(nameof(GetQuizForInstructor), new { quizId = quiz.Id, instructorId = dto.RequestingInstructorId }, quiz);
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }

        [HttpPut("{quizId}")]
        public async Task<ActionResult<QuizDTO>> UpdateQuiz(string quizId, [FromBody] UpdateQuizDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var quiz = await _quizService.UpdateQuizAsync(quizId, dto, dto.RequestingInstructorId);
                return Ok(quiz);
            }
            catch (ArgumentException ex) { return NotFound(ex.Message); }
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }

        [HttpPost("models")]
        public async Task<ActionResult<QuizModelDTO>> AddQuizModel([FromForm] UploadQuizModelCsvDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (dto.CsvFile == null || dto.CsvFile.Length == 0) return BadRequest("CSV file is required.");
            try
            {
                var quizModel = await _quizService.AddQuizModelAsync(dto, dto.RequestingInstructorId);
                return Ok(quizModel);
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
            catch (InvalidOperationException ex) { return Conflict(ex.Message); }
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }

        [HttpDelete("{quizId}")]
        public async Task<IActionResult> DeleteQuiz(string quizId, [FromQuery] string instructorId)
        {
            if (string.IsNullOrEmpty(instructorId)) return BadRequest("Instructor ID is required.");
            try
            {
                await _quizService.DeleteQuizAsync(quizId, instructorId);
                return NoContent();
            }
            catch (ArgumentException ex) { return NotFound(ex.Message); }
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }

        [HttpGet("instructor/my-quizzes")]
        public async Task<ActionResult<IReadOnlyList<QuizDTO>>> GetMyQuizzesAsInstructor([FromQuery] string instructorId)
        {
            if (string.IsNullOrEmpty(instructorId)) return BadRequest("Instructor ID is required.");
            try
            {
                var quizzes = await _quizService.GetQuizzesByInstructorAsync(instructorId);
                return Ok(quizzes);
            }
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }

        [HttpGet("{quizId}/instructor-view")]
        public async Task<ActionResult<QuizDTO>> GetQuizForInstructor(string quizId, [FromQuery] string instructorId)
        {
            if (string.IsNullOrEmpty(instructorId)) return BadRequest("Instructor ID is required.");
            try
            {
                var quiz = await _quizService.GetQuizByIdForInstructorAsync(quizId, instructorId);
                return Ok(quiz);
            }
            catch (ArgumentException ex) { return NotFound(ex.Message); }
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }

        [HttpGet("group/{groupId}/instructor-view")]
        public async Task<ActionResult<IReadOnlyList<QuizDTO>>> GetQuizzesByGroupForInstructor(string groupId, [FromQuery] string instructorId)
        {
            if (string.IsNullOrEmpty(instructorId)) return BadRequest("Instructor ID is required.");
            try
            {
                var quizzes = await _quizService.GetQuizzesByGroupIdForInstructorAsync(groupId, instructorId);
                return Ok(quizzes);
            }
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }

        [HttpGet("attempts/{attemptId}/instructor-view")]
        public async Task<ActionResult<QuizAttemptResultDTO>> GetStudentAttemptForInstructor(string attemptId, [FromQuery] string instructorId)
        {
            if (string.IsNullOrEmpty(instructorId)) return BadRequest("Instructor ID is required.");
            try
            {
                var attemptResult = await _quizService.GetStudentAttemptDetailsForInstructorAsync(attemptId, instructorId);
                return Ok(attemptResult);
            }
            catch (ArgumentException ex) { return NotFound(ex.Message); }
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }

        [HttpGet("{quizId}/attempts/all")]
        public async Task<ActionResult<IReadOnlyList<QuizAttemptResultDTO>>> GetAllAttemptsForQuiz(string quizId, [FromQuery] string instructorId)
        {
            if (string.IsNullOrEmpty(instructorId)) return BadRequest("Instructor ID is required.");
            try
            {
                var attempts = await _quizService.GetAllAttemptsForQuizAsync(quizId, instructorId);
                return Ok(attempts);
            }
            catch (ArgumentException ex) { return NotFound(ex.Message); }
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }


        [HttpGet("student/available")]
        public async Task<ActionResult<IReadOnlyList<StudentQuizListItemDTO>>> GetAvailableQuizzesForStudent([FromQuery] string studentId)
        {
            if (string.IsNullOrEmpty(studentId)) return BadRequest("Student ID is required.");
            try
            {
                var quizzes = await _quizService.GetAvailableQuizzesForStudentAsync(studentId);
                return Ok(quizzes);
            }
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }

        public class StudentIdDTO { public string StudentId { get; set; } }

        [HttpPost("{quizId}/start")]
        public async Task<ActionResult<StudentQuizAttemptDTO>> StartQuizAttempt(string quizId, [FromBody] StudentIdDTO studentDto)
        {
            if (string.IsNullOrEmpty(studentDto.StudentId)) return BadRequest("Student ID is required.");
            try
            {
                var attemptDto = await _quizService.StartQuizAttemptAsync(quizId, studentDto.StudentId);
                return Ok(attemptDto);
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
            catch (InvalidOperationException ex) { return Conflict(ex.Message); }
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }

        [HttpGet("{quizId}/resume")]
        public async Task<ActionResult<StudentQuizAttemptDTO>> ResumeQuizAttempt(string quizId, [FromQuery] string studentId)
        {
            if (string.IsNullOrEmpty(studentId)) return BadRequest("Student ID is required.");
            try
            {
                var attemptDto = await _quizService.GetStudentQuizAttemptInProgressAsync(quizId, studentId);
                return Ok(attemptDto);
            }
            catch (ArgumentException ex) { return NotFound(ex.Message); }
            catch (InvalidOperationException ex) { return Conflict(ex.Message); }
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }

        [HttpPost("attempt/submit")]
        public async Task<ActionResult<QuizAttemptResultDTO>> SubmitQuizAttempt([FromBody] StudentQuizSubmissionDTO submissionDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var resultDto = await _quizService.SubmitQuizAttemptAsync(submissionDto, submissionDto.RequestingStudentId);
                return Ok(resultDto);
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
            catch (InvalidOperationException ex) { return Conflict(ex.Message); }
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }

        [HttpGet("attempt/{attemptId}/result")]
        public async Task<ActionResult<QuizAttemptResultDTO>> GetStudentQuizAttemptResult(string attemptId, [FromQuery] string studentId)
        {
            if (string.IsNullOrEmpty(studentId)) return BadRequest("Student ID is required.");
            try
            {
                var resultDto = await _quizService.GetStudentQuizAttemptResultAsync(attemptId, studentId);
                return Ok(resultDto);
            }
            catch (ArgumentException ex) { return NotFound(ex.Message); }
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }
    }
}