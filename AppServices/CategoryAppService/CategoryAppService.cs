using AutoMapper;
using MyBudgetManagement.Dtos;
using MyBudgetManagement.Dtos.Categories;
using MyBudgetManagement.Models.Categories;
using MyBudgetManagement.Repositories.Categories;

namespace MyBudgetManagement.AppService.CategoryAppService;

public class CategoryAppService : ICategoryAppService
{
    private readonly ICategoryRepository _categoryRepository;

    private readonly IMapper _mapper;

    public CategoryAppService(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }
    
    public async Task<List<CategoryDto>> GetListCategoryAsync()
    {
        var listCategory = await _categoryRepository.GetListCategoriesAsync();
        if (listCategory != null)
        {
            var listCategoryDto = _mapper.Map<List<CategoryDto>>(listCategory);
            foreach (var cat in listCategoryDto)
            {
                cat.Balance = await _categoryRepository.GetBalanceAsync(cat.UserBalanceId);
                cat.Spent = await _categoryRepository.GetSpentAsync(cat.Id);
            }
            return listCategoryDto.OrderByDescending(x=>x.Spent).ToList();
        }

        return new List<CategoryDto>();
    }

    public async Task<List<SelectedDto>> GetListCategorySelectedAsync()
    {
        var listCategory = await GetListCategoryAsync();
        var selectedList = _mapper.Map<List<SelectedDto>>(listCategory);
        return selectedList;
    }

    public async Task<int> CreateCategoryAsync(Category category)
    {
        var check = await _categoryRepository.CreateCategoriesAsync(category);
        return check;

    }
}