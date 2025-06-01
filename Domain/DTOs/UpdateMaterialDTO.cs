using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class UpdateMaterialDTO
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? FileUrl { get; set; }
    }
}
