using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class StudentQuizAttemptDTO
    {
        public string AttemptId { get; set; } = default!;
        public string QuizId { get; set; } = default!;
        public string QuizModelId { get; set; } = default!;
        public string QuizTitle { get; set; } = default!;
        public DateTime StartTime { get; set; }
        public DateTime? ExpectedEndTime => StartTime.AddMinutes(DurationMinutes);
        public int DurationMinutes { get; set; }
        public bool ShuffleQuestions { get; set; }
        public List<QuestionDTO> Questions { get; set; } = new List<QuestionDTO>();
        public QuizAttemptStatus Status { get; set; }
    }
}
