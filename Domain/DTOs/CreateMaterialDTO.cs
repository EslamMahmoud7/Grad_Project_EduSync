using Domain.Entities;
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
        [StringLength(200)]
        public string Title { get; set; } = default!;

        public string? Description { get; set; }

        [Required]
        [StringLength(500)]
        [Url(ErrorMessage = "Please enter a valid URL.")]
        public string FileUrl { get; set; } = default!;

        [Required]
        public MaterialType Type { get; set; }

        [Required]
        public string GroupId { get; set; } = default!;

        [Required]
        public string UploadingInstructorId { get; set; } = default!;
    }
}
