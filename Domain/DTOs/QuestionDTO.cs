using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class QuestionDTO
    {
        public string Id { get; set; } = default!;
        public string Text { get; set; } = default!;
        public int Points { get; set; }
        public QuestionType Type { get; set; }
        public List<QuestionOptionDTO> Options { get; set; } = new List<QuestionOptionDTO>();
    }
}
