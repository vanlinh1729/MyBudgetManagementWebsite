namespace MyBudgetManagement.Dtos.UserBalances;

public class CreateOrUpdateUserBalanceDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public decimal Balance { get; set; }
}