using Microsoft.AspNetCore.Mvc;

namespace MyBudgetManagement.Controllers;

public class UserBalanceController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}