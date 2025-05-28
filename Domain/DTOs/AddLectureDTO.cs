using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class AddLectureDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime Duration { get; set; }
        public string Feedback { get; set; }
        //public Course Course { get; set; }
        public int CourseId { get; set; }
        //public string CourseName { get; set; }
        public string Type { get; set; }
        public DateTime Time { get; set; }
        public string Video { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
    }
}
