namespace MyBudgetManagement.Models;

public class UserBalance
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public decimal Balance { get; set; }
}