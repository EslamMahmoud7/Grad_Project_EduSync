using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class UpdateMaterialDTO
    {
        [StringLength(200)]
        public string? Title { get; set; }

        public string? Description { get; set; } 
        [StringLength(500)]
        [Url(ErrorMessage = "Please enter a valid URL.")]
        public string? FileUrl { get; set; }

        public MaterialType? Type { get; set; }

        [Required]
        public string UpdatingInstructorId { get; set; } = default!;
    }
}
