namespace MyBudgetManagement.ViewModels.Categories;

public class CreateCategoryViewModel
{
    public string Name { get; set; }
    public decimal Budget { get; set; }
    public Guid UserBalanceId { get; set; }
}