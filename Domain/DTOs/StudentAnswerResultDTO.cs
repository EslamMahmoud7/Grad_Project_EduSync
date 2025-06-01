using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class StudentAnswerResultDTO
    {
        public string QuestionId { get; set; } = default!;
        public string QuestionText { get; set; } = default!;
        public string? SelectedOptionId { get; set; }
        public string? SelectedOptionText { get; set; }
        public string? CorrectOptionId { get; set; }
        public string? CorrectOptionText { get; set; }
        public bool IsCorrect { get; set; }
        public int PointsAwarded { get; set; }
        public int PointsPossibleForQuestion { get; set; }
    }
}
