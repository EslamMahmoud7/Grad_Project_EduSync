using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class StudentQuizSubmissionDTO
    {
        [Required]
        public string AttemptId { get; set; } = default!;
        [Required]
        public List<StudentAnswerSubmitDTO> Answers { get; set; } = new List<StudentAnswerSubmitDTO>();
    }
}
