using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class AssignmentDTO
    {
        public string Id { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public DateTime DueDate { get; set; }
        public AssignmentStatus Status { get; set; }
        public string GroupId { get; set; } = default!;
        public string GroupLabel { get; set; } = default!;
        public string CourseTitle { get; set; } = default!;
        public int Grade { get; set; } = 0;
    }
}
