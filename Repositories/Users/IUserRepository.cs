using MyBudgetManagement.Models;
namespace MyBudgetManagement.Repositories;

public interface IUserRepository
{
    Task<User> CreateOrUpdateUser(User user);
    Task<string> DeleteUser(Guid id);
    Task<List<User>> GetAllUsers();
    Task<User> GetUserById(Guid id);
    Task<User> GetUserByEmail(string email);
    Task<bool> CheckPasswordAsync(string email, string password);
    List<Role> GetUserRoleByUserId(Guid id);


}