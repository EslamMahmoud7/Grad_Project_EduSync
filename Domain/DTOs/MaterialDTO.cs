using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class MaterialDTO
    {
        public string Id { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public string FileUrl { get; set; } = default!;
        public MaterialType Type { get; set; }
        public string TypeLabel => Type.ToString();
        public string GroupId { get; set; } = default!;
        public string GroupLabel { get; set; } = default!;
        public string CourseTitle { get; set; } = default!;
        public string UploadedById { get; set; } = default!;
        public string UploadedByName { get; set; } = default!;
        public DateTime DateUploaded { get; set; }
    }
}
