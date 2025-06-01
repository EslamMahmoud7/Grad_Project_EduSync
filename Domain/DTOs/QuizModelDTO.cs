using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class QuizModelDTO
    {
        public string Id { get; set; } = default!;
        public string QuizId { get; set; } = default!;
        public string ModelIdentifier { get; set; } = default!;
        public DateTime DateCreated { get; set; }
        public List<QuestionDTO>? Questions { get; set; }
    }
}
