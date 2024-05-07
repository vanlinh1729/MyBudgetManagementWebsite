namespace MyBudgetManagement.Models.Categories;

public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Budget { get; set; }
    public Guid UserBalanceId { get; set; }
    public Guid UserId { get; set; }
    public ICollection<Transaction> Transactions { get; set; }
    public User User { get; set; }
    
}