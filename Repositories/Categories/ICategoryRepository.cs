using MyBudgetManagement.Models.Categories;

namespace MyBudgetManagement.Repositories.Categories;

public interface ICategoryRepository
{
    Task<List<Category>> GetListCategoriesAsync();
    Task<int> CreateCategoriesAsync(Category category);
    Task<decimal> GetBalanceAsync(Guid userbalacneid);
    Task<decimal> GetSpentAsync(Guid categoryId);
}