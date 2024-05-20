using AutoMapper;
using MyBudgetManagement.AppService.MD5Service;
using MyBudgetManagement.AppService.MD5Service.FileHandle;
using MyBudgetManagement.Dtos.AccountProfiles;
using MyBudgetManagement.Repositories.AccountProfiles;

namespace MyBudgetManagement.AppServices.AccountProfile;

public class AccountProfileAppService : IAccountProfileAppService
{
    private readonly IAccountProfileRepository _accountProfileRepository;
    private readonly IMapper _mapper;
    private readonly GetCurrentUser _current;
    private readonly FileHandler _fileHandler;

    public AccountProfileAppService(IAccountProfileRepository accountProfileRepository, IMapper mapper, GetCurrentUser current)
    {
        _accountProfileRepository = accountProfileRepository;
        _mapper = mapper;
        _current = current;
    }

    public async Task<AccountProFileViewDto> GetAccountProfileDtoAsync()
    {
        var user = await _current.GetCurrentUserModel();
        var userId = user.Id;
        
       var accountProfile =await  _accountProfileRepository.GetAccountProfileByUserId(userId);
       var accountProfileDto = _mapper.Map<AccountProFileViewDto>(accountProfile);
       accountProfileDto.FirstName = _current.GetCurrentUserModel().Result.FirstName;
       accountProfileDto.LastName = _current.GetCurrentUserModel().Result.LastName;
       return accountProfileDto;
    }

    public async Task<AccountProFileViewDto> CreateAccountProfileAsync(Models.AccountProfile accountProfile)
    {
        var accountprofile = await _accountProfileRepository.CreateAccountProfile(accountProfile);
        return _mapper.Map<AccountProFileViewDto>(accountProfile);
    }
}