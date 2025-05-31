using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class LectureDTO
    {
        public string Time { get; set; } = default!;
        public string Subject { get; set; } = default!;
        public string Doctor { get; set; } = default!;
    }
}
