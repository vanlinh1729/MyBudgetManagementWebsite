using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyBudgetManagement.AppService.AuthSevice;
using MyBudgetManagement.AppService.UserAppService;
using MyBudgetManagement.Models;
using MyBudgetManagement.ViewModels.Login;

namespace MyBudgetManagement.Controllers;

[Microsoft.AspNetCore.Components.Route("Login")]
public class LoginController : Controller
{
    private readonly IUserAppService _userAppService;
    private readonly JwtProvider _jwtProvider;

    public LoginController(IUserAppService userAppService, JwtProvider jwtProvider)
    {
        _userAppService = userAppService;
        _jwtProvider = jwtProvider;
    }

    public IActionResult Index()
    {
        return View();
    }
   
    public IActionResult Register()
    {
        return View();
    }
    
    
   
}   