using System.ComponentModel.DataAnnotations;

    namespace Domain.Entities
    {
        public class QuizModel
        {
            public string Id { get; set; } = Guid.NewGuid().ToString();

            [Required]
            public string QuizId { get; set; } = default!;
            public Quiz Quiz { get; set; } = default!;

            [Required]
            [StringLength(50)]
            public string ModelIdentifier { get; set; } = default!;
            public DateTime DateCreated { get; set; } = DateTime.UtcNow;

            public ICollection<Question> Questions { get; set; } = new List<Question>();
            public ICollection<StudentQuizAttempt> AttemptsForThisModel { get; set; } = new List<StudentQuizAttempt>();
        }
    }
