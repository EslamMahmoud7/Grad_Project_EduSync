using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class CourseDto
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string InstructorName { get; set; }
        public string InstructorEmail { get; set; }
        public string ResourceLink { get; set; }
        public int Credits { get; set; }
        public int Progress { get; set; }
        public DateTime NextDeadline { get; set; }
    }
}
