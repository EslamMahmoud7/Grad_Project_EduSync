using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Instructor
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string? PhoneNumber { get; set; }
        public ICollection<Group> Groups { get; set; } = new List<Group>();
    }
}