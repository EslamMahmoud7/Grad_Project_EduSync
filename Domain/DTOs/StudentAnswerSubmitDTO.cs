using System.ComponentModel.DataAnnotations;

public class StudentAnswerSubmissionDTO
{
    [Required]
    public string QuestionId { get; set; } = default!;
    public string? SelectedOptionId { get; set; }
}