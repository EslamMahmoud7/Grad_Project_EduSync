using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ScheduleItem
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public string Day { get; set; }
        public string Time { get; set; }
        public string Subject { get; set; }
        public string Room { get; set; }
        public string DoctorEmail { get; set; }

        public int StudentProfileId { get; set; }
        public StudentProfile StudentProfile { get; set; }
    }

}
