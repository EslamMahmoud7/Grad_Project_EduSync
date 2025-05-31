using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs
{
    public class GroupEnrollmentDTO
    {
        [Required]
        public string StudentId { get; set; } = default!;

        [Required]
        public string GroupId { get; set; } = default!;
    }
}