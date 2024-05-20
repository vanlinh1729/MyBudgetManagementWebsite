using MyBudgetManagement.Shared.AccountProfiles;

namespace MyBudgetManagement.Models;

public class AccountProfile
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string? Avatar { get; set; }
    public string? Age { get; set; }
    public string? Address { get; set; }
    public Gender Gender { get; set; }
    public Currencies Currency { get; set; }
}