using MyBudgetManagement.Shared.Transactions;

namespace MyBudgetManagement.ViewModels.Transaction;

public class CreateOrUpdateTransactionViewModel
{
    public string Title { get; set; }
    public decimal Amount { get; set; }
    public Guid CategoryId { get; set; }
    public string Description { get; set; }
    public TransactionType Type { get; set; }
    public DateTime Date { get; set; }
}