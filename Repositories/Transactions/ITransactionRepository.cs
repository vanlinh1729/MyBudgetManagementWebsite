
using MyBudgetManagement.Models;
using MyBudgetManagement.Shared.Transactions;

namespace MyBudgetManagement.Repositories;

public interface ITransactionRepository
{
    Task<Transaction> GetTransactionById(Guid id);
    Task<List<Transaction>> GetAllTransaction(Guid userId);
    Task<List<Transaction>> GetAllTransactionOfUserByType(Guid userId, TransactionType type);
    Task<List<Transaction>> GetAllTransactionByUserId(Guid userId);
    
    Task<int> CreateOrUpdateTransactionAndUpdateUserBalance(Transaction transaction);
    Task<Transaction> DeleteTransaction(Guid id);
}