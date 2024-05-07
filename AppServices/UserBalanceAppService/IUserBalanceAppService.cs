using MyBudgetManagement.Dtos.UserBalances;
using MyBudgetManagement.Models;
using MyBudgetManagement.ViewModels.UserBalances;

namespace MyBudgetManagement.AppService.UserBalanceAppService;

public interface IUserBalanceAppService
{
    UserBalanceDto CreateOrUpdateUserBalance(UserBalance userBalance);
    UserBalance GetUserBalance();
}