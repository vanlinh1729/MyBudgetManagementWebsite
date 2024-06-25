using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using MyBudgetManagement.AppService.AuthSevice;
using MyBudgetManagement.Models;
using NuGet.Protocol.Plugins;

namespace MyBudgetManagement.Middlewares;

public class ValidateTokenMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IActionDescriptorCollectionProvider _actionDescriptorProvider;
    private readonly IConfiguration _configuration;

    public ValidateTokenMiddleware(RequestDelegate next, IActionDescriptorCollectionProvider actionDescriptorProvider, IConfiguration configuration)
    {
        _next = next;
        _actionDescriptorProvider = actionDescriptorProvider;
        _configuration = configuration;
    }


    public async Task Invoke(HttpContext context)
    {
        try
        {
            var endpoint = context.GetEndpoint();
            if (endpoint?.RequestDelegate.Target?.GetType().FullName.StartsWith("Hangfire.Server") == true)
            {
                await _next(context);
                return;
            }

            if (!context.GetEndpoint().Metadata.Where(x => x.GetType().Name == "Login").Any())
            {
                await _next(context);
                return;
            }

            if (context.Request.Cookies.TryGetValue("JwtToken", out var  token))
            {
                // Lấy header Authorization từ yêu cầu
                string authorizationHeader = context.Request.Headers["Authorization"];

                // Kiểm tra xem header Authorization có tồn tại không
                if (!string.IsNullOrEmpty(authorizationHeader))
                {
                    /*
                    // Phân tích token từ header Authorization
                    var token = authorizationHeader.Replace("Bearer ", "");
                    */

                    // Kiểm tra token để lấy các claims (phát sinh từ token)
                    var claimsPrincipal = GetClaimsPrincipalFromToken(token);

                    // Kiểm tra xem các claims có chứa role yêu cầu hay không
                    if (claimsPrincipal != null && IsInRequiredRole(context, claimsPrincipal))
                    {
                        // Nếu token hợp lệ và chứa role yêu cầu, tiếp tục xử lý yêu cầu
                        context.Request.Headers["IsAuthenticated"] = "true";
                        context.Request.Headers["Roles"] = claimsPrincipal.ToString();
                        await _next(context);
                        return;
                    }
                    else
                    {
                        context.Items["IsAuthenticated"] = false;
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("Ban khong co quyen truy cap");
                    }
                }
            }
        }
        catch (Exception e)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Ban khong co quyen truy cap " + e.Message);

        }

    }

// Phương thức để phân tích token và trả về ClaimsPrincipal
    private ClaimsPrincipal GetClaimsPrincipalFromToken(string token)
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

        // Giải mã mã thông báo
        SecurityToken validatedToken;
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);

        return principal;
    }
// Phương thức để kiểm tra xem ClaimsPrincipal có chứa role yêu cầu không
    private bool IsInRequiredRole(HttpContext context, ClaimsPrincipal claimsPrincipal)
    {
        var endpoint = context.GetEndpoint();
        if (endpoint?.Metadata?.GetMetadata<IAuthorizeData>() is IAuthorizeData authorizeData)
        {
            // Lấy danh sách các quyền yêu cầu từ AuthorizeData
            var requiredRoles = authorizeData.Roles != null? authorizeData.Roles.Split(",") : null;

            if (requiredRoles != null)
            {
                foreach (var role in requiredRoles)
                {
                    if (!claimsPrincipal.IsInRole(role.Trim()))
                    {
                        return false;
                    }
                }
            }
            // Kiểm tra xem ClaimsPrincipal có chứa bất kỳ quyền nào trong danh sách yêu cầu hay không
           

            // Nếu tất cả các quyền yêu cầu đều tồn tại trong ClaimsPrincipal, trả về true
            return true;
        }

        // Nếu không có AuthorizeData, mặc định là hợp lệ
        return true;
    }
    
}