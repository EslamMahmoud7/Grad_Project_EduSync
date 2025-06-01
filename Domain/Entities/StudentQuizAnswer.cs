// Domain/Entities/StudentQuizAnswer.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class StudentQuizAnswer
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string StudentQuizAttemptId { get; set; } = default!;
        public StudentQuizAttempt StudentQuizAttempt { get; set; } = default!;

        [Required]
        public string QuestionId { get; set; } = default!;
        public Question Question { get; set; } = default!;
        public string? SelectedOptionId { get; set; }
        public QuestionOption? SelectedOption { get; set; }
        public bool? IsCorrect { get; set; }
        public int? PointsAwarded { get; set; }
    }
}