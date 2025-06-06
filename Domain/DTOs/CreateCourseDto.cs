using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class CreateCourseDto
    {
        public string? Code { get; set; } = "BIS01";
        [Required]
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public int Credits { get; set; }
        public int Level { get; set; }
    }
}
