using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AdminProfile
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }

        public string Department { get; set; }
        public string AvatarUrl { get; set; }

        public ICollection<Announcement> Announcements { get; set; }
        public ICollection<Notification> Notifications { get; set; }
    }
}
