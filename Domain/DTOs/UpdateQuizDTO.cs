using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class UpdateQuizDTO
    {
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
