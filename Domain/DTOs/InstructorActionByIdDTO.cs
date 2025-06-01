using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class InstructorActionByIdDTO
    {
        [Required]
        public string InstructorId { get; set; } = default!;
    }
}
