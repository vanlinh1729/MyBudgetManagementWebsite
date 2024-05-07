using MyBudgetManagement.Shared.Transactions;

namespace MyBudgetManagement.Dtos.Transactions;

public class TransactionDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid? CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public string Email { get; set; }
    public string Title { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public TransactionType Type { get; set; }
    public DateTime Date { get; set; }
}