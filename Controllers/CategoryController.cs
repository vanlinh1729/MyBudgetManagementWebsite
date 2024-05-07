using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyBudgetManagement.AppService.CategoryAppService;
using MyBudgetManagement.AppService.MD5Service;
using MyBudgetManagement.AppService.UserBalanceAppService;
using MyBudgetManagement.Dtos;
using MyBudgetManagement.Models;
using MyBudgetManagement.Models.Categories;
using MyBudgetManagement.ViewModels.Categories;

namespace MyBudgetManagement.Controllers;

[ApiController]
[Route("/api/category")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryAppService _categoryAppService;
    private readonly IUserBalanceAppService _userBalanceAppService;
    private readonly IMapper _mapper;
    private readonly GetCurrentUser _current;

    public CategoryController(ICategoryAppService categoryAppService, IUserBalanceAppService userBalanceAppService, IMapper mapper, GetCurrentUser current)
    {
        _categoryAppService = categoryAppService;
        _userBalanceAppService = userBalanceAppService;
        _mapper = mapper;
        _current = current;
    }

    // GET
    [HttpGet]
    public async Task<IActionResult> GetListCategory()
    {
       var listCategory =  await _categoryAppService.GetListCategoryAsync();
       return Ok(listCategory);
    } 
    
    [HttpGet("/api/category/selected")]
    public async Task<IActionResult> GetListSelectedCategory()
    {
       var listSelectedCat =  await _categoryAppService.GetListCategorySelectedAsync();
       return Ok(listSelectedCat);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryViewModel categoryviewmodel)
    {
        var currentUId = _current.GetCurrentUserModel().Result.Id;
        var currentUser =  _mapper.Map<User>(_current.GetCurrentUserModel().Result);
        var currentUserBalanceId = _userBalanceAppService.GetUserBalance().Id;

        var category = new Category
        {
            Id = Guid.NewGuid(),
            Budget = categoryviewmodel.Budget,
            Name = categoryviewmodel.Name,
            UserId = currentUId,
            UserBalanceId = currentUserBalanceId
        };
        var cateDto = await _categoryAppService.CreateCategoryAsync(category);
       return Ok(cateDto);
    }
    
}