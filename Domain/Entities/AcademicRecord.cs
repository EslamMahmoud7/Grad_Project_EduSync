using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AcademicRecord
    {
        public string Id { get; set; }
        public string CourseCode { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }
        public string Grade { get; set; }
        public string Term { get; set; }
        public string Instructor { get; set; }

        public int StudentProfileId { get; set; }
        public StudentProfile StudentProfile { get; set; }
    }

}
