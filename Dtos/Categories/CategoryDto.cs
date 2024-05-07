namespace MyBudgetManagement.Dtos.Categories;

public class CategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Budget { get; set; }
    public Guid UserBalanceId { get; set; }
    public decimal Balance { get; set; }
    public decimal Spent { get; set; }
}