using MyBudgetManagement.Dtos.Users;
using MyBudgetManagement.Models;

namespace MyBudgetManagement.AppService.UserAppService;

public interface IUserAppService
{
    Task<UserDto> GetUserById(Guid id);
    Task<bool> IsAuthenticated(string email, string password);
    Task<UserDto> RegisterAccount(User userModel);
    Task<UserDto> GetUserByEmail(string email);
    List<Role> GetRoleByUserId(Guid id);
}