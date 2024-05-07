using MyBudgetManagement.AppService.AuthSevice;
using MyBudgetManagement.AppService.UserAppService;
using MyBudgetManagement.Dtos.Users;
using MyBudgetManagement.Models;

namespace MyBudgetManagement.AppService.MD5Service;

public class GetCurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly JwtProvider _jwtProvider;
    private readonly IUserAppService _userAppService;

    public GetCurrentUser(IHttpContextAccessor httpContextAccessor, JwtProvider jwtProvider, IUserAppService userAppService)
    {
        _httpContextAccessor = httpContextAccessor;
        _jwtProvider = jwtProvider;
        _userAppService = userAppService;
    }

    public async Task<UserDto> GetCurrentUserModel()
    {
        string token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"]
            .FirstOrDefault()?.Split(" ").Last();
        var currentUserId = _jwtProvider.GetUserIdFromJwtToken(token);
        var currentUser = await _userAppService.GetUserById(currentUserId);
        return currentUser;
    }
}