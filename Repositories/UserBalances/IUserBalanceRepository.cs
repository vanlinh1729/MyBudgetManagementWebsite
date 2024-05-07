using MyBudgetManagement.Models;

namespace MyBudgetManagement.Repositories;

public interface IUserBalanceRepository
{
    UserBalance CreateOrUpdateUserBalance(UserBalance userBalance);
    Task<UserBalance> GetUserBalanceById(Guid id);
    UserBalance GetUserBalanceByUserId(Guid userId);
    
}