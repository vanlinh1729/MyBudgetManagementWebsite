using MyBudgetManagement.Models.Categories;
using MyBudgetManagement.Shared.Transactions;

namespace MyBudgetManagement.Models;

public class Transaction
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    
    public Guid? CategoryId { get; set; }
    public string Title { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public TransactionType Type { get; set; }
    public DateTime Date { get; set; }
    public User User { get; set; }
    public Category? Category { get; set; }

}