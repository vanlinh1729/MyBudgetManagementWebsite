using MyBudgetManagement.Models;

namespace MyBudgetManagement.Repositories.AccountProfiles;

public interface IAccountProfileRepository
{
    Task<AccountProfile> CreateAccountProfile(AccountProfile accountProfile);
    Task<AccountProfile> GetAccountProfileById(Guid id);
    Task<AccountProfile> GetAccountProfileByUserId(Guid id);
}