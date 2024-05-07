namespace MyBudgetManagement.ViewModels.UserBalances;

public class CreateOrUpdateUserBalanceViewModel
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public decimal Balance { get; set; }
}