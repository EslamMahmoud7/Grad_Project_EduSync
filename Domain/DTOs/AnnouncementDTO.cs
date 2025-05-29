using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class AnnouncementDTO
    {
        public string Title { get; set; } = default!;
        public string Message { get; set; } = default!;
        public DateTime Date { get; set; }
    }
}
