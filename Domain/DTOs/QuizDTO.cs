using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class QuizDTO
    {
        public string Id { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public string GroupId { get; set; } = default!;
        public string GroupLabel { get; set; } = default!;
        public string InstructorId { get; set; } = default!;
        public string InstructorName { get; set; } = default!;
        public DateTime DueDate { get; set; }
        public int DurationMinutes { get; set; }
        public bool ShuffleQuestions { get; set; }
        public int MaxAttempts { get; set; }
        public bool IsPublished { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public List<QuizModelDTO> QuizModels { get; set; } = new List<QuizModelDTO>();
        public int NumberOfModels => QuizModels?.Count ?? 0;
    }
}

