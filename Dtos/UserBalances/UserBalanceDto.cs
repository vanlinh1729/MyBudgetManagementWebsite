namespace MyBudgetManagement.Dtos.UserBalances;

public class UserBalanceDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public decimal Balance { get; set; }
}