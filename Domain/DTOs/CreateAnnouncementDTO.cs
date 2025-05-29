using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class CreateAnnouncementDTO
    {
        public string Title { get; set; } = default!;
        public string Message { get; set; } = default!;
    }
}
