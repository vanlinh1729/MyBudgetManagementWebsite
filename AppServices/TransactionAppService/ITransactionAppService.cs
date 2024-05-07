using MyBudgetManagement.Dtos;
using MyBudgetManagement.Dtos.Transactions;
using MyBudgetManagement.Models;

namespace MyBudgetManagement.AppService.TransactionAppService;

public interface ITransactionAppService
{
    Task<List<TransactionDto>> GetListTransactionAsync();
    Task<int> CreateTransaction(Transaction model);
}