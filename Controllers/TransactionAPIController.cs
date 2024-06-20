using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBudgetManagement.AppService.MD5Service;
using MyBudgetManagement.AppService.TransactionAppService;
using MyBudgetManagement.Repositories;
using MyBudgetManagement.Repositories.Categories;
using MyBudgetManagement.Shared.Transactions;
using MyBudgetManagement.Shared.Users;
using MyBudgetManagement.ViewModels.Transaction;
using Transaction = MyBudgetManagement.Models.Transaction;

namespace MyBudgetManagement.Controllers;

[ApiController]
[Route("api/transaction/")]
public class TransactionAPIController : ControllerBase
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ITransactionAppService _transactionAppService;
    private readonly ICategoryRepository _categoryRepository;
    private readonly GetCurrentUser _current;
    private readonly IMapper _mapper;

    public TransactionAPIController(ITransactionRepository transactionRepository,
        ITransactionAppService transactionAppService, ICategoryRepository categoryRepository, GetCurrentUser current,
        IMapper mapper)
    {
        _transactionRepository = transactionRepository;
        _transactionAppService = transactionAppService;
        _categoryRepository = categoryRepository;
        _current = current;
        _mapper = mapper;
    }

    // GET
    [Authorize(Roles = nameof(Role.User))]
    [HttpGet("/transaction/getlist")]
    public async Task<IActionResult> GetListTransactionAsync()
    {
        var listTransactions = _transactionAppService.GetListTransactionAsync().Result;
        return listTransactions != null ? Ok(listTransactions) : BadRequest("Errorrrrr");
    }

    [Authorize(Roles = nameof(Role.User))]
    [HttpPost("/transaction/create")]
    public async Task<IActionResult> CreateTransaction([FromBody] CreateOrUpdateTransactionViewModel model)
    {
        if (model.CategoryId != Guid.Empty)
        {
            model.Type = TransactionType.Expense;
             var category = await _categoryRepository.GetCategoryByIdAsync(model.CategoryId);
            var catExpense = _categoryRepository.GetSpentAsync(model.CategoryId).Result;
            if (model.Amount + catExpense >= category.Budget)
            {
                var user = await _current.GetCurrentUserModel();
                var smtpServer = "smtp.gmail.com";
                var port = 465;
                var username = "searchm5v@gmail.com";
                var password = "dnbklsyltyscqial";
                var enableSsl = true;

                var mailService = new MailService(smtpServer, port, username, password, enableSsl);

                var fromName = "My Budget Management";
                var fromEmail = "searchm5v@gmail.com";
                var toName = user.FirstName + " " + user.LastName;
                var toEmail = user.Email;
                string subject = "Budget Exceeded Alert";
                 string body = $@"
    <!DOCTYPE html>
    <html lang='en'>
    <head>
        <meta charset='UTF-8'>
        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        <title>Budget Exceeded Notification</title>
        <style>
            body {{
                font-family: Arial, sans-serif;
                background-color: #f4f4f4;
                margin: 0;
                padding: 0;
            }}
            .container {{
                width: 100%;
                max-width: 600px;
                margin: 0 auto;
                background-color: #ffffff;
                padding: 20px;
                box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
            }}
            .header {{
                text-align: center;
                padding: 10px 0;
                background-color: #ff4c4c;
                color: white;
            }}
            .content {{
                padding: 20px;
            }}
            .footer {{
                text-align: center;
                padding: 10px 0;
                background-color: #f4f4f4;
                color: #666666;
                font-size: 12px;
            }}
            h1 {{
                color: #333333;
            }}
            p {{
                color: #666666;
                line-height: 1.5;
            }}
            .button {{
                display: inline-block;
                padding: 10px 20px;
                margin: 20px 0;
                font-size: 16px;
                color: white;
                background-color: #ff4c4c;
                text-decoration: none;
                border-radius: 5px;
            }}
            .button:hover {{
                background-color: #e63939;
            }}
        </style>
    </head>
    <body>
        <div class='container'>
            <div class='header'>
                <h1>Budget Exceeded Alert</h1>
            </div>
            <div class='content'>
                <h1>Hi, {user.FirstName}!</h1>
                <p>We wanted to let you know that you have exceeded your budget for the <strong>{category.Name}</strong> category.</p>
                <p>Here are the details:</p>
                <ul>
                    <li>Budget Limit: {category.Budget}</li>
                    <li>Current Spending: {catExpense + model.Amount}</li>
                </ul>
                <p>Please review your spending and make necessary adjustments to stay within your budget.</p>
                <a href='https://mybudgetmanagement.nguyenvanlinh.io.vn/transaction' class='button'>View Budget Details</a>
            </div>
            <div class='footer'>
                 <p>&copy; 2024 My BudgetManagemeny. All rights reserved.</p>
                    <p>Hanoi, Vietnam</p>
            </div>
        </div>
    </body>
    </html>";
                mailService.SendEmail(fromName, fromEmail, toName, toEmail, subject, body,
                    true); 
            }
        }

        var transactionModel = _mapper.Map<Transaction>(model);

        var transaction = await _transactionAppService.CreateTransaction(transactionModel);
        if (transaction != 0)
            return Ok(transaction);
        return BadRequest(new { Message = "Tạo transaction thất bại" });
    }
}