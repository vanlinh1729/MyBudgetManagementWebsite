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

public class AddAuthorizationHeaderMiddleWare
{
    private readonly RequestDelegate _next;
    private readonly IActionDescriptorCollectionProvider _actionDescriptorProvider;
    private readonly IConfiguration _configuration;

    public AddAuthorizationHeaderMiddleWare(RequestDelegate next, IActionDescriptorCollectionProvider actionDescriptorProvider, IConfiguration configuration)
    {
        _next = next;
        _actionDescriptorProvider = actionDescriptorProvider;
        _configuration = configuration;
    }


    public async Task Invoke(HttpContext context)
    {
        // Lấy header Authorization từ yêu cầu
        string token = context.Request.Headers["Authorization"].ToString();

        // Kiểm tra xem header Authorization có tồn tại không
        if (!string.IsNullOrEmpty(token))
        {
            context.Request.Headers["Authorization"] = $"Bearer {token.Split(' ')[1]}";

        }

        await _next(context);
    }
}
    