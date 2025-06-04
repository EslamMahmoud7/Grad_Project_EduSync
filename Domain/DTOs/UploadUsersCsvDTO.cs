using Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class UploadUsersCsvDTO
    {
        [Required]
        public IFormFile CsvFile { get; set; } = default!;
        [Required]
        public UserRole AssignedRoleForAll { get; set; }
    }
    public class ParsedUserCsvRowDTO
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public int OriginalRowNumber { get; set; }
    }
}
