using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MyBudgetManagement.AppService.UserAppService;
using MyBudgetManagement.Dtos.Users;
using MyBudgetManagement.Shared.Users;

namespace MyBudgetManagement.AppService.AuthSevice;

public class JwtProvider
{
    private readonly IConfiguration _configuration;
    private readonly IUserAppService _userAppService;
    private readonly string token;

    public JwtProvider(string token)
    {
        this.token = token;
    }

    public JwtProvider(IConfiguration configuration,IUserAppService userAppService)
    {
        _configuration = configuration;
        _userAppService = userAppService;
    }

    public string GenerateJwtToken(UserDto userDto)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration.GetSection("Jwt")["SecretKey"]);
        var roles = _userAppService.GetRoleByUserId(userDto.Id);
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userDto.Id.ToString()),
            new Claim(ClaimTypes.Email, userDto.Email)
        };

        // Thêm các claim về vai trò vào danh sách claim
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.Name.ToString()));
        }
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Audience = _configuration.GetSection("Jwt")["Audience"],
            Issuer = _configuration.GetSection("Jwt")["Issuer"],
            Expires = DateTime.UtcNow.AddDays(7), // Token expiration time
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    public Guid GetUserIdFromJwtToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration.GetSection("Jwt")["SecretKey"]);

        // Thiết lập các cài đặt cho việc giải mã mã thông báo
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = true,
            ValidAudience = _configuration.GetSection("Jwt")["Audience"],
            ClockSkew = TimeSpan.Zero
        };


            SecurityToken validatedToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);
    
            var userIdClaim = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return userId;
            }
            else
            {
                // Log lỗi khi không tìm thấy UserID hoặc không thể chuyển đổi thành Guid
                Console.WriteLine("Không tìm thấy UserID hoặc không thể chuyển đổi thành Guid.");
                return Guid.Empty;
            }
    }
    public string GetUserEmailFromJwtToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration.GetSection("Jwt")["SecretKey"]);

        // Thiết lập các cài đặt cho việc giải mã mã thông báo
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = true,
            ValidAudience = _configuration.GetSection("Jwt")["Audience"],
            ClockSkew = TimeSpan.Zero
        };


            SecurityToken validatedToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);
    
            var emailClaim = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
            if (emailClaim != null)
            {
                return emailClaim.Value;
            }
            else
            {
                // Log lỗi khi không tìm thấy UserID hoặc không thể chuyển đổi thành Guid
                Console.WriteLine("Không tìm thấy UserID hoặc không thể chuyển đổi thành Guid.");
                return String.Empty;
            }
    }


    public bool CheckAuthorizationByJwtToken(string token, Role role)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("Jwt")["SecretKey"]);

            // Thiết lập các cài đặt cho việc giải mã mã thông báo
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ClockSkew = TimeSpan.Zero
            };

            // Giải mã mã thông báo
            SecurityToken validatedToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);

            // Kiểm tra các claim để xác định quyền truy cập
            var claimsIdentity = principal.Identity as ClaimsIdentity;
            if (claimsIdentity == null || !claimsIdentity.IsAuthenticated)
            {
                return false;
            }

            // Kiểm tra claim về vai trò (nếu cần)
            var roleClaim = claimsIdentity.FindAll(ClaimTypes.Role).Select(c=>c.Value);
            return roleClaim.Contains(role.ToString());
            
        }
        catch (Exception ex)
        {
            // Xử lý ngoại lệ nếu cần
            return false;
        }
    }
    
    public bool ValidateToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("Jwt")["SecretKey"]);

            // Thiết lập các cài đặt cho việc giải mã mã thông báo
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            // Giải mã mã thông báo
            SecurityToken validatedToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);
            return true;
        }
        catch (Exception ex)
        {
            // Xử lý ngoại lệ nếu cần
            return false;
        }
    }
}