using Microsoft.AspNetCore.Mvc;

namespace MyBudgetManagement.Controllers;

public class AccountProfileController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}