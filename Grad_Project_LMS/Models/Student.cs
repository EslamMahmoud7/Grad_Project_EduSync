using System.ComponentModel.DataAnnotations;

namespace Grad_Project_LMS.Models
{
    public class Student : BaseClass
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Address { get; set; }
        public string NationalID { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime FirstLogin { get; set; }

    }
}
