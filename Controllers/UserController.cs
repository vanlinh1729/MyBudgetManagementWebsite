using AutoMapper;
using AutoMapper.Internal.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyBudgetManagement.AppService.AuthSevice;
using MyBudgetManagement.AppService.UserAppService;
using MyBudgetManagement.AppService.UserBalanceAppService;
using MyBudgetManagement.Dtos.UserBalances;
using MyBudgetManagement.Dtos.Users;
using MyBudgetManagement.Models;
using MyBudgetManagement.Repositories.Applications;
using MyBudgetManagement.ViewModels.Login;
using MyBudgetManagement.ViewModels.UserBalances;
using Role = MyBudgetManagement.Shared.Users.Role;

namespace MyBudgetManagement.Controllers;

[ApiController]
[Route("api/user/")]
public class UsersController : ControllerBase
{
    private readonly IUserAppService _userAppService;
    private readonly IUserBalanceAppService _userBalanceAppService;
    private readonly IApplicationRepository _applicationRepository;
    private readonly IConfiguration _configuration;
    private readonly JwtProvider _jwtProvider;
    private readonly IMapper _mapper;

    public UsersController(IMapper mapper,IUserAppService userAppService,IApplicationRepository applicationRepository, IConfiguration configuration,JwtProvider jwtProvider, IUserBalanceAppService userBalanceAppService)
    {
        _userAppService = userAppService;
        _applicationRepository = applicationRepository;
        _configuration = configuration;
        _jwtProvider = jwtProvider;
        _userBalanceAppService = userBalanceAppService;
        _mapper = mapper;
    }
    

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUserById(Guid id)
    {
        var userDto = await _userAppService.GetUserById(id);
        if (userDto == null) return NotFound();
        return Ok(userDto);
    }
    
    [Authorize(Roles = nameof(Role.Admin))]
    [HttpGet("/getallprocedures")]
    public string GetStoredProcedure()
    {
        return _applicationRepository.GetStoredProcedure();
    }
    
    [Authorize(Roles = nameof(Role.User))]
    [HttpGet("/getuserbalance")]
    public IActionResult GetUserBalance()
    {
        var userBalance = _userBalanceAppService.GetUserBalance();
        if (userBalance.Id != Guid.Empty)
        {
            return Ok(userBalance);
        }
        return BadRequest(new { Message = "User nay chua co UserBalance" });
        
    }
    
    [Authorize(Roles = nameof(Role.User))]
    [HttpPost("/createuserbalance")]
    public IActionResult CreateUserBalance([FromBody] CreateOrUpdateUserBalanceViewModel model)
    {
        try
        {
            // Mapping dữ liệu từ ViewModel sang UserBalanceDto
            var userBalance = _mapper.Map<UserBalance>(model);
            var createdUserBalance = _userBalanceAppService.CreateOrUpdateUserBalance(userBalance);
            var userBalanceDto = _mapper.Map<UserBalanceDto>(createdUserBalance);

            // Kiểm tra nếu Id của UserBalanceDto trả về từ dịch vụ là rỗng
            if (userBalanceDto.Id == Guid.Empty)
            {
                return BadRequest(new { Message = "User này đã có UserBalance, không thể tạo thêm" });
            }

            return Ok(userBalanceDto); // Trả về đối tượng UserBalanceDto thành công
        }
        catch (Exception ex)
        {
            // Xử lý lỗi và trả về lỗi tương ứng với status code 500 Internal Server Error
            return StatusCode(StatusCodes.Status500InternalServerError, new { Message = ex.Message });
        }
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel model)
    {
       //kiem tra tai khoan
        var user = await _userAppService.GetUserByEmail(model.Email);
        //dang nhap
        if (user.Id != Guid.Empty && await _userAppService.IsAuthenticated(model.Email, model.Password))
        {
            //tao token
            var token = _jwtProvider.GenerateJwtToken(user);
            
            bool isValid = _jwtProvider.ValidateToken(token);
            
            return Ok(new { Token = token });
        }

        return Unauthorized();
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
    {
        var userDto = _mapper.Map<User>(model);
        var user = await _userAppService.RegisterAccount(userDto);
        if (user.Id != Guid.Empty && _userAppService.GetUserByEmail(model.Email).Result.Id != Guid.Empty)
        {
            return Ok(user);
        }
        return BadRequest(new { Message = "Email này đã có người sử dụng, vui lòng thử email khác để đăng kí" });
    }
}