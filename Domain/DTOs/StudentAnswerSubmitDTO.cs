using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class StudentAnswerSubmitDTO
    {
        [Required]
        public string QuestionId { get; set; } = default!;
        public string? SelectedOptionId { get; set; }
    }
}
