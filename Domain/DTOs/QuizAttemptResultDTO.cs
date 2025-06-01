using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class QuizAttemptResultDTO
    {
        public string AttemptId { get; set; } = default!;
        public string QuizId { get; set; } = default!;
        public string QuizTitle { get; set; } = default!;
        public string StudentId { get; set; } = default!;
        public string StudentName { get; set; } = default!;
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public double? Score { get; set; }
        public int TotalPointsPossible { get; set; }
        public QuizAttemptStatus Status { get; set; }
        public List<StudentAnswerResultDTO> AnswerResults { get; set; } = new List<StudentAnswerResultDTO>();
    }
}
