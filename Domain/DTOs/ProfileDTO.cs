namespace Domain.DTOs
{
    public class ProfileDTO
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string JoinedDate { get; set; }
        public string Institution { get; set; }
        public int TotalCourses { get; set; }
        public double GPA { get; set; }
        public string Status { get; set; }
        public string AvatarUrl { get; set; }
        public List<string> Achievements { get; set; }
        public List<string> RecentActivity { get; set; }
        public List<string> SocialLinks { get; set; }
    }
}
