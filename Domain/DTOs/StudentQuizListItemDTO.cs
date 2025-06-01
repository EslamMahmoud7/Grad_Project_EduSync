using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class StudentQuizListItemDTO
    {
        public string QuizId { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public string CourseTitle { get; set; } = default!;
        public string GroupLabel { get; set; } = default!;
        public DateTime DueDate { get; set; }
        public int DurationMinutes { get; set; }
        public int MaxAttempts { get; set; }
        public int AttemptsMade { get; set; }
        public bool CanAttempt => AttemptsMade < MaxAttempts;
        public string? LastAttemptStatus { get; set; }
    }
}
