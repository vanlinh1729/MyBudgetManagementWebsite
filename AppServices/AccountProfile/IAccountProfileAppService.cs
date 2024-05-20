using MyBudgetManagement.Dtos.AccountProfiles;

namespace MyBudgetManagement.AppServices.AccountProfile;

public interface IAccountProfileAppService
{
    Task<AccountProFileViewDto> GetAccountProfileDtoAsync();
    Task<AccountProFileViewDto> CreateAccountProfileAsync(Models.AccountProfile accountProfile);
}