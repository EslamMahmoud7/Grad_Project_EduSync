namespace Domain.DTOs
{
    public class InstructorDashboardCountsDTO
    {
        public int TotalGroups { get; set; }
        public int TotalAssignmentsInGroups { get; set; }
        public int TotalTodayClasses { get; set; }
        public int TotalUniqueStudentsInGroups { get; set; }

    }
}