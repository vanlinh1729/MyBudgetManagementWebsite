using System.Security.Claims;
using AutoMapper;
using AutoMapper.Internal.Mappers;
using Microsoft.AspNetCore.Authorization;
using MyBudgetManagement.AppService.AuthSevice;
using MyBudgetManagement.Dtos.UserBalances;
using MyBudgetManagement.Models;
using MyBudgetManagement.Repositories;
using MyBudgetManagement.ViewModels.UserBalances;
using Role = MyBudgetManagement.Shared.Users.Role;

namespace MyBudgetManagement.AppService.UserBalanceAppService;

public class UserBalanceAppService :IUserBalanceAppService
{
    private readonly IMapper _mapper;
    private readonly IUserBalanceRepository _userBalanceRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly JwtProvider _jwtProvider;

    public UserBalanceAppService(IMapper mapper, IUserBalanceRepository userBalanceRepository, IHttpContextAccessor httpContextAccessor, JwtProvider jwtProvider)
    {
        _mapper = mapper;
        _userBalanceRepository = userBalanceRepository;
        _httpContextAccessor = httpContextAccessor;
        _jwtProvider = jwtProvider;
    }


    public UserBalanceDto CreateOrUpdateUserBalance(UserBalance userBalance)
    {
       var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
       if (token != null)
       {
           var userId = _jwtProvider.GetUserIdFromJwtToken(token);
           if (userId != Guid.Empty)
           {
               userBalance.Id = Guid.NewGuid();
               userBalance.UserId = userId;
               var userbalance = _userBalanceRepository.CreateOrUpdateUserBalance(userBalance);
               return  _mapper.Map<UserBalanceDto>(userbalance);
           }
           
       }
       else
       {
           return new UserBalanceDto();
       }

       return new UserBalanceDto();
    }

    public UserBalance GetUserBalance()
    {
        var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (token != null)
        {
            var userId = _jwtProvider.GetUserIdFromJwtToken(token);
            if (userId != Guid.Empty)
            {
                return _userBalanceRepository.GetUserBalanceByUserId(userId);
            }
        }
        return new UserBalance();
    }
}