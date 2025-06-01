using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class QuestionOption
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string QuestionId { get; set; } = default!;
        public Question Question { get; set; } = default!;

        [Required]
        public string Text { get; set; } = default!;
        public bool IsCorrect { get; set; } = false;
    }
}