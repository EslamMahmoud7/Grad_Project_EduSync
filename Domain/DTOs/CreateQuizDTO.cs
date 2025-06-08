using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs
{
    public class CreateQuizDTO
    {
        [Required]
        public string RequestingInstructorId { get; set; } = default!;

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = default!;

        public string? Description { get; set; }

        [Required]
        public string GroupId { get; set; } = default!;

        [Required]
        public DateTime DueDate { get; set; }

        [Range(1, 600)]
        public int DurationMinutes { get; set; } = 60;

        public bool ShuffleQuestions { get; set; } = false;

        [Range(1, 10)]
        public int MaxAttempts { get; set; } = 1;

        public bool IsPublished { get; set; } = false;
    }
}