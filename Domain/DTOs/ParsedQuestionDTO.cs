using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class ParsedQuestionDTO
    {
        public string QuestionText { get; set; } = default!;
        public List<string> Options { get; set; } = new List<string>();
        public int CorrectOptionIndex { get; set; }
        public int Points { get; set; } = 1;
    }
}
