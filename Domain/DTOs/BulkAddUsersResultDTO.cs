using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class BulkAddUsersResultDTO
    {
        public int TotalRowsAttempted { get; set; }
        public int SuccessfullyAddedCount { get; set; }
        public List<string> ErrorMessages { get; set; } = new List<string>();
    }
}
