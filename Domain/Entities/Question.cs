using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Question
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string QuizModelId { get; set; } = default!;
        public QuizModel QuizModel { get; set; } = default!;

        [Required]
        public string Text { get; set; } = default!;
        public int Points { get; set; } = 1;
        public QuestionType Type { get; set; } = QuestionType.MultipleChoiceSingleAnswer;

        public ICollection<QuestionOption> Options { get; set; } = new List<QuestionOption>();
        public ICollection<StudentQuizAnswer> StudentAnswers { get; set; } = new List<StudentQuizAnswer>();
    }
}