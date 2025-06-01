using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class QuestionOptionDTO
    {
        public string Id { get; set; } = default!;
        public string Text { get; set; } = default!;
        public bool? IsCorrect { get; set; }
    }
}
