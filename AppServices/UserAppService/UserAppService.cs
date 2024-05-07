using System.Security.Cryptography;
using AutoMapper;
using MyBudgetManagement.AppService.MD5Service;
using MyBudgetManagement.Dtos.Users;
using MyBudgetManagement.Models;
using MyBudgetManagement.Repositories;

namespace MyBudgetManagement.AppService.UserAppService;


public class UserAppService : IUserAppService
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public UserAppService(IMapper mapper,IUserRepository userRepository)
    {
        _mapper = mapper;
        _userRepository = userRepository;
    }
    
    public async Task<UserDto> GetUserById(Guid id)
    {
       var user = await _userRepository.GetUserById(id);
       return _mapper.Map<UserDto>(user);
    }
    public async Task<UserDto> GetUserByEmail(string email)
    {
       var user = await _userRepository.GetUserByEmail(email);
       if (user.Id != Guid.Empty)
       {
           return _mapper.Map<UserDto>(user);

       }

       return new UserDto();
    }

    public List<Role> GetRoleByUserId(Guid id)
    {
        var listRoles= _userRepository.GetUserRoleByUserId(id);
        return listRoles;
    }

    public async Task<bool> IsAuthenticated(string email, string password)
    {
        var isAuthenticated = await _userRepository.CheckPasswordAsync(email, Md5Encrypt.Encrypt(password));
        return isAuthenticated;
    }

    public async Task<UserDto> RegisterAccount(User userModel)
    {
        userModel.Id = Guid.NewGuid();
        userModel.Password = Md5Encrypt.Encrypt(userModel.Password);
        var user = await _userRepository.CreateOrUpdateUser(userModel);
        if (user.Id != Guid.Empty)
            return _mapper.Map<UserDto>(user);
        return new UserDto();
    }
}
