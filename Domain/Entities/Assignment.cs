using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Assignment
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public DateTime DueDate { get; set; }
        public AssignmentStatus Status { get; set; } = AssignmentStatus.Pending;
        public string GroupId { get; set; } = default!;
        public Group Group { get; set; } = default!;
    }
}
