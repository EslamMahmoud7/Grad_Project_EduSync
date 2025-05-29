using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Lecture
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime Date { get; set; }
        public string Topic { get; set; } = default!;
        public string InstructorName { get; set; } = default!;
        public string CourseId { get; set; } = default!;
        public Course Course { get; set; } = default!;
    }
}
