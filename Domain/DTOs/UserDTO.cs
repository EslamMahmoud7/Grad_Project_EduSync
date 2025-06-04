using Domain.Entities;

public class UserDTO
{
    public string Id { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string FullName => $"{FirstName} {LastName}";
    public string Email { get; set; } = default!;
    public UserRole Role { get; set; }
    public string RoleLabel => Role.ToString();
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? AvatarUrl { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Institution { get; set; }
}