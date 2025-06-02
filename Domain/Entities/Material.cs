// Domain/Entities/Material.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Material
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = default!;

        public string? Description { get; set; }

        [Required]
        [StringLength(500)]
        public string FileUrl { get; set; } = default!;

        public MaterialType Type { get; set; } = MaterialType.Link;

        [Required]
        public string GroupId { get; set; } = default!;
        public virtual Group Group { get; set; } = default!;

        [Required]
        public string UploadedById { get; set; } = default!;
        public virtual User UploadedBy { get; set; } = default!;

        public DateTime DateUploaded { get; set; } = DateTime.UtcNow;
    }
}