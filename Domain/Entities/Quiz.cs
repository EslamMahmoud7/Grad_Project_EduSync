// Domain/Entities/Quiz.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Quiz
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = default!;

        public string? Description { get; set; }

        [Required]
        public string GroupId { get; set; } = default!;
        public Group Group { get; set; } = default!;

        [Required]
        public string InstructorId { get; set; } = default!;
        public User Instructor { get; set; } = default!;

        public DateTime DueDate { get; set; }
        public int DurationMinutes { get; set; }
        public bool ShuffleQuestions { get; set; } = false;
        public int MaxAttempts { get; set; } = 1;
        public bool IsPublished { get; set; } = false;
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime? DateModified { get; set; }

        public ICollection<QuizModel> QuizModels { get; set; } = new List<QuizModel>();
        public ICollection<StudentQuizAttempt> Attempts { get; set; } = new List<StudentQuizAttempt>();
    }
}