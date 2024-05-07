using System.Transactions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBudgetManagement.AppService.TransactionAppService;
using MyBudgetManagement.Dtos.Transactions;
using MyBudgetManagement.Repositories;
using MyBudgetManagement.Shared.Transactions;
using MyBudgetManagement.Shared.Users;
using MyBudgetManagement.ViewModels.Transaction;
using Transaction = MyBudgetManagement.Models.Transaction;

namespace MyBudgetManagement.Controllers;

[ApiController]
[Route("api/transaction/")]
public class TransactionController : ControllerBase
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ITransactionAppService _transactionAppService;
    private readonly IMapper _mapper;

    public TransactionController(ITransactionRepository transactionRepository, ITransactionAppService transactionAppService, IMapper mapper)
    {
        _transactionRepository = transactionRepository;
        _transactionAppService = transactionAppService;
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
        }
        var transactionModel = _mapper.Map<Transaction>(model);

        var transaction =await _transactionAppService.CreateTransaction(transactionModel);
        if (transaction != 0)
        {
            return Ok(transaction);
        }
        else
        {
            return BadRequest(new { Message = "Tạo transaction thất bại" });
        }
       
    }

}