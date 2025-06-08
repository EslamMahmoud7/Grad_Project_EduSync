using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs
{
    public class UpdateQuizDTO
    {
        [Required]
        public string RequestingInstructorId { get; set; } = default!;

        [StringLength(200)]
        public string? Title { get; set; }

        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }

        [Range(1, 600)]
        public int? DurationMinutes { get; set; }

        public bool? ShuffleQuestions { get; set; }

        [Range(1, 10)]
        public int? MaxAttempts { get; set; }

        public bool? IsPublished { get; set; }
    }
}