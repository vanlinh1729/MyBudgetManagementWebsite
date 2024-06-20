using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.AppService.UserBalanceAppService;
using MyBudgetManagement.EntityFrameworkCore;
using MyBudgetManagement.Models;
using MyBudgetManagement.Shared.Transactions;

namespace MyBudgetManagement.Repositories;

public class TransationRepository : ITransactionRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IUserBalanceAppService _userBalanceAppService;

    public TransationRepository(ApplicationDbContext context, IUserBalanceAppService userBalanceAppService)
    {
        _context = context;
        _userBalanceAppService = userBalanceAppService;
    }

    public Task<Transaction> GetTransactionById(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Transaction>> GetAllTransaction(Guid userId)
    {
        var listTransactions = await _context.Transactions.Where(x=>x.UserId == userId).ToListAsync();
        foreach (var transaction in listTransactions)
        {
            transaction.Category = _context.Category.FirstOrDefault(x => x.Id == transaction.CategoryId);
            transaction.User = _context.Users.FirstOrDefault(x => x.Id == transaction.UserId);

        }
        return listTransactions;
    }

    public Task<List<Transaction>> GetAllTransactionOfUserByType(Guid userId, TransactionType type)
    {
        throw new NotImplementedException();
    }

    public Task<List<Transaction>> GetAllTransactionByUserId(Guid userId)
    {
        throw new NotImplementedException();
    }

    public async Task<int> CreateOrUpdateTransactionAndUpdateUserBalance(Transaction transaction)
    {
        if (transaction.CategoryId == Guid.Empty)
        {
            transaction.CategoryId = null;
            transaction.User = _context.Users.SingleOrDefault(x => x.Id == transaction.UserId);
        }
        _context.Transactions.Add(transaction);

        var userBalance = _context.UserBalances.SingleOrDefault(x => x.Id == _userBalanceAppService.GetUserBalance().Id);
        if (userBalance != null)
        {
            if (transaction.Type == TransactionType.Income)
            {
                userBalance.Balance += transaction.Amount;
            }
            else
            {
                userBalance.Balance -= transaction.Amount;
            }
        }

        return await _context.SaveChangesAsync();        
    }
    public Task<Transaction> DeleteTransaction(Guid id)
    {
        throw new NotImplementedException();
    }
}