using MyBudgetManagement.Shared.AccountProfiles;

namespace MyBudgetManagement.Dtos.AccountProfiles;

public class AccountProFileViewDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Avatar { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Address { get; set; }
    public Gender Gender { get; set; }
    public Currencies Currency { get; set; }
}