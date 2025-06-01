using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class StudentQuizAttempt
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string StudentId { get; set; } = default!;
        public User Student { get; set; } = default!;

        [Required]
        public string QuizId { get; set; } = default!;
        public Quiz Quiz { get; set; } = default!;

        [Required]
        public string QuizModelId { get; set; } = default!;
        public QuizModel QuizModel { get; set; } = default!;

        public int AttemptNumber { get; set; } = 1;
        public DateTime StartTime { get; set; } = DateTime.UtcNow;
        public DateTime? EndTime { get; set; }
        public double? Score { get; set; }
        public QuizAttemptStatus Status { get; set; } = QuizAttemptStatus.InProgress;

        public ICollection<StudentQuizAnswer> Answers { get; set; } = new List<StudentQuizAnswer>();
    }
}