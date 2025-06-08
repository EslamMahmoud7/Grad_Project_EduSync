using System.ComponentModel.DataAnnotations;

public class StudentQuizSubmissionDTO
{
    [Required]
    public string RequestingStudentId { get; set; } = default!;

    [Required]
    public string AttemptId { get; set; } = default!;

    public List<StudentAnswerSubmissionDTO> Answers { get; set; } = new List<StudentAnswerSubmissionDTO>();
}