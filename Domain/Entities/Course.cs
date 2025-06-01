using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Course
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Code { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public int Credits { get; set; }

        public string ResourceLink { get; set; } = string.Empty;
        public int Level { get; set; } = 1;
        public ICollection<Group> Groups { get; set; } = new List<Group>();
    }
}
