using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class CreateMaterialDTO
    {
        [Required]
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        [Required]
        public string FileUrl { get; set; } = default!;
        [Required]
        public string GroupId { get; set; } = default!;
    }
}
