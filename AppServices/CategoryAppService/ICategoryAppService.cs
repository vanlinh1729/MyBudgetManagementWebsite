using MyBudgetManagement.Dtos;
using MyBudgetManagement.Dtos.Categories;
using MyBudgetManagement.Models;
using MyBudgetManagement.Models.Categories;

namespace MyBudgetManagement.AppService.CategoryAppService;

public interface ICategoryAppService
{
    Task<List<CategoryDto>> GetListCategoryAsync();
    Task<List<SelectedDto>> GetListCategorySelectedAsync();
    Task<int> CreateCategoryAsync(Category category);
    
}