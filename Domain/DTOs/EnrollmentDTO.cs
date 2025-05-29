using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class EnrollmentDTO
    {
        public string StudentId { get; set; } = default!;
        public string CourseId { get; set; } = default!;
    }
}
