using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class GroupStudent
    {
        public string GroupId { get; set; } = default!;
        public Group Group { get; set; } = default!;

        public string StudentId { get; set; } = default!;
        public User Student { get; set; } = default!;
    }

}
