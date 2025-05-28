using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AdminProfile
    {
        public required string Id { get; set; }
        public required string UserId { get; set; }
        public required User User { get; set; }

        public required string Department { get; set; }
        public string AvatarUrl { get; set; } = string.Empty;

        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
