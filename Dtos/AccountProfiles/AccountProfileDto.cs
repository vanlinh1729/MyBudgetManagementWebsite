using MyBudgetManagement.Shared.AccountProfiles;

namespace MyBudgetManagement.Dtos.AccountProfiles;

public interface AccountProfileDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public IFormFile Avatar { get; set; }
    public string Age { get; set; }
    public string Address { get; set; }
    public Gender Gender { get; set; }
    public Currencies Currency { get; set; }
}