using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class UploadQuizModelCsvDTO
    {
        [Required]
        public string QuizId { get; set; } = default!;
        [Required]
        [StringLength(50)]
        public string ModelIdentifier { get; set; } = default!;
        [Required]
        public IFormFile CsvFile { get; set; } = default!;
    }
}
