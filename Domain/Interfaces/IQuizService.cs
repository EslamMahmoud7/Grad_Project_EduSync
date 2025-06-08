using Domain.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces.IServices
{
    public interface IQuizService
    {
        Task<QuizDTO> CreateQuizAsync(CreateQuizDTO dto, string instructorId);
        Task<QuizDTO> UpdateQuizAsync(string quizId, UpdateQuizDTO dto, string requestingInstructorId);
        Task<QuizModelDTO> AddQuizModelAsync(UploadQuizModelCsvDTO dto, string instructorId);
        Task DeleteQuizAsync(string quizId, string instructorId);
        Task<QuizDTO> GetQuizByIdForInstructorAsync(string quizId, string instructorId);
        Task<IReadOnlyList<QuizDTO>> GetQuizzesByGroupIdForInstructorAsync(string groupId, string instructorId);
        Task<IReadOnlyList<QuizDTO>> GetQuizzesByInstructorAsync(string instructorId);
        Task<IReadOnlyList<StudentQuizListItemDTO>> GetAvailableQuizzesForStudentAsync(string studentId);
        Task<StudentQuizAttemptDTO> StartQuizAttemptAsync(string quizId, string studentId);
        Task<StudentQuizAttemptDTO> GetStudentQuizAttemptInProgressAsync(string quizId, string studentId);
        Task<QuizAttemptResultDTO> SubmitQuizAttemptAsync(StudentQuizSubmissionDTO submissionDto, string studentId);
        Task<QuizAttemptResultDTO> GetStudentQuizAttemptResultAsync(string attemptId, string studentId);
        Task<QuizAttemptResultDTO> GetStudentAttemptDetailsForInstructorAsync(string attemptId, string instructorId);
        Task<IReadOnlyList<QuizAttemptResultDTO>> GetAllAttemptsForQuizAsync(string quizId, string instructorId);
    }
}