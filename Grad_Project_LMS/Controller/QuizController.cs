//// Grad_Project_LMS/Controller/QuizController.cs
//using Domain.DTOs;
//using Domain.Interfaces.IServices;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Security.Claims;
//using System.Threading.Tasks;

//namespace Grad_Project_LMS.Controller
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class QuizController : ControllerBase
//    {
//        private readonly IQuizService _quizService;

//        public QuizController(IQuizService quizService)
//        {
//            _quizService = quizService;
//        }

//        private string GetCurrentUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException("User ID not found in token.");
//        private bool IsInRole(string role) => User.IsInRole(role);

//        [HttpPost]
//        public async Task<ActionResult<QuizDTO>> CreateQuiz([FromBody] CreateQuizDTO dto)
//        {
//            if (!ModelState.IsValid) return BadRequest(ModelState);
//            try
//            {
//                var instructorId = GetCurrentUserId();
//                var quiz = await _quizService.CreateQuizAsync(dto, instructorId);
//                return CreatedAtAction(nameof(GetQuizForInstructor), new { quizId = quiz.Id }, quiz);
//            }
//            catch (ArgumentException ex) { return BadRequest(ex.Message); }
//            catch (UnauthorizedAccessException ex) { return Forbid(ex.Message); }
//            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
//        }

//        [HttpPut("{quizId}")]
//        public async Task<ActionResult<QuizDTO>> UpdateQuiz(string quizId, [FromBody] UpdateQuizDTO dto)
//        {
//            if (!ModelState.IsValid) return BadRequest(ModelState);
//            try
//            {
//                var instructorId = GetCurrentUserId();
//                var quiz = await _quizService.UpdateQuizAsync(quizId, dto, instructorId);
//                return Ok(quiz);
//            }
//            catch (ArgumentException ex) { return NotFound(ex.Message); }
//            catch (UnauthorizedAccessException ex) { return Forbid(ex.Message); }
//            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
//        }

//        [HttpPost("models")]
//        public async Task<ActionResult<QuizModelDTO>> AddQuizModel([FromForm] UploadQuizModelCsvDTO dto) 
//        {
//            if (!ModelState.IsValid) return BadRequest(ModelState);
//            if (dto.CsvFile == null || dto.CsvFile.Length == 0)
//                return BadRequest("CSV file is required.");
//            try
//            {
//                var instructorId = GetCurrentUserId();
//                var quizModel = await _quizService.AddQuizModelAsync(dto, instructorId);
//                return Ok(quizModel);
//            }
//            catch (ArgumentException ex) { return BadRequest(ex.Message); }
//            catch (InvalidOperationException ex) { return Conflict(ex.Message); }
//            catch (UnauthorizedAccessException ex) { return Forbid(ex.Message); }
//            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
//        }

//        [HttpDelete("{quizId}")]
//        public async Task<IActionResult> DeleteQuiz(string quizId)
//        {
//            try
//            {
//                var instructorId = GetCurrentUserId();
//                await _quizService.DeleteQuizAsync(quizId, instructorId);
//                return NoContent();
//            }
//            catch (ArgumentException ex) { return NotFound(ex.Message); }
//            catch (UnauthorizedAccessException ex) { return Forbid(ex.Message); }
//            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
//        }

//        [HttpGet("{quizId}/instructor-view")]
//        public async Task<ActionResult<QuizDTO>> GetQuizForInstructor(string quizId)
//        {
//            try
//            {
//                var instructorId = GetCurrentUserId();
//                var quiz = await _quizService.GetQuizByIdForInstructorAsync(quizId, instructorId);
//                return Ok(quiz);
//            }
//            catch (ArgumentException ex) { return NotFound(ex.Message); }
//            catch (UnauthorizedAccessException ex) { return Forbid(ex.Message); }
//            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
//        }

//        [HttpGet("group/{groupId}/instructor-view")]
//        public async Task<ActionResult<IReadOnlyList<QuizDTO>>> GetQuizzesByGroupForInstructor(string groupId)
//        {
//            try
//            {
//                var instructorId = GetCurrentUserId();
//                var quizzes = await _quizService.GetQuizzesByGroupIdForInstructorAsync(groupId, instructorId);
//                return Ok(quizzes);
//            }
//            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
//        }

//        [HttpGet("instructor/my-quizzes")]
//        public async Task<ActionResult<IReadOnlyList<QuizDTO>>> GetMyQuizzesAsInstructor()
//        {
//            try
//            {
//                var instructorId = GetCurrentUserId();
//                var quizzes = await _quizService.GetQuizzesByInstructorAsync(instructorId);
//                return Ok(quizzes);
//            }
//            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
//        }


//        [HttpGet("attempts/{attemptId}/instructor-view")]
//        public async Task<ActionResult<QuizAttemptResultDTO>> GetStudentAttemptForInstructor(string attemptId)
//        {
//            try
//            {
//                var instructorId = GetCurrentUserId();
//                var attemptResult = await _quizService.GetStudentAttemptDetailsForInstructorAsync(attemptId, instructorId);
//                return Ok(attemptResult);
//            }
//            catch (ArgumentException ex) { return NotFound(ex.Message); }
//            catch (UnauthorizedAccessException ex) { return Forbid(ex.Message); }
//            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
//        }

//        [HttpGet("{quizId}/attempts/all")]
//        public async Task<ActionResult<IReadOnlyList<QuizAttemptResultDTO>>> GetAllAttemptsForQuiz(string quizId)
//        {
//            try
//            {
//                var instructorId = GetCurrentUserId();
//                var attempts = await _quizService.GetAllAttemptsForQuizAsync(quizId, instructorId);
//                return Ok(attempts);
//            }
//            catch (ArgumentException ex) { return NotFound(ex.Message); }
//            catch (UnauthorizedAccessException ex) { return Forbid(ex.Message); }
//            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
//        }



//        [HttpGet("student/available")]
//        public async Task<ActionResult<IReadOnlyList<StudentQuizListItemDTO>>> GetAvailableQuizzesForStudent()
//        {
//            try
//            {
//                var studentId = GetCurrentUserId();
//                var quizzes = await _quizService.GetAvailableQuizzesForStudentAsync(studentId);
//                return Ok(quizzes);
//            }
//            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
//        }

//        [HttpPost("{quizId}/start")]
//        public async Task<ActionResult<StudentQuizAttemptDTO>> StartQuizAttempt(string quizId)
//        {
//            try
//            {
//                var studentId = GetCurrentUserId();
//                var attemptDto = await _quizService.StartQuizAttemptAsync(quizId, studentId);
//                return Ok(attemptDto);
//            }
//            catch (ArgumentException ex) { return BadRequest(ex.Message); }
//            catch (InvalidOperationException ex) { return Conflict(ex.Message); }
//            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
//        }

//        [HttpGet("{quizId}/resume")]
//        public async Task<ActionResult<StudentQuizAttemptDTO>> ResumeQuizAttempt(string quizId)
//        {
//            try
//            {
//                var studentId = GetCurrentUserId();
//                var attemptDto = await _quizService.GetStudentQuizAttemptInProgressAsync(quizId, studentId);
//                return Ok(attemptDto);
//            }
//            catch (ArgumentException ex) { return NotFound(ex.Message); }
//            catch (InvalidOperationException ex) { return Conflict(ex.Message); }
//            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
//        }


//        [HttpPost("attempt/submit")]
//        public async Task<ActionResult<QuizAttemptResultDTO>> SubmitQuizAttempt([FromBody] StudentQuizSubmissionDTO submissionDto)
//        {
//            if (!ModelState.IsValid) return BadRequest(ModelState);
//            try
//            {
//                var studentId = GetCurrentUserId();
//                var resultDto = await _quizService.SubmitQuizAttemptAsync(submissionDto, studentId);
//                return Ok(resultDto);
//            }
//            catch (ArgumentException ex) { return BadRequest(ex.Message); }
//            catch (InvalidOperationException ex) { return Conflict(ex.Message); }
//            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
//        }

//        [HttpGet("attempt/{attemptId}/result")]
//        public async Task<ActionResult<QuizAttemptResultDTO>> GetStudentQuizAttemptResult(string attemptId)
//        {
//            try
//            {
//                var studentId = GetCurrentUserId();
//                var resultDto = await _quizService.GetStudentQuizAttemptResultAsync(attemptId, studentId);
//                return Ok(resultDto);
//            }
//            catch (ArgumentException ex) { return NotFound(ex.Message); }
//            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
//        }
//    }
//}