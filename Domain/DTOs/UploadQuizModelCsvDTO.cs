using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs
{
    public class UploadQuizModelCsvDTO
    {
        [Required]
        public string RequestingInstructorId { get; set; } = default!;

        [Required]
        public string QuizId { get; set; } = default!;

        [Required]
        [StringLength(50)]
        public string ModelIdentifier { get; set; } = default!;

        [Required]
        public IFormFile CsvFile { get; set; } = default!;
    }
}