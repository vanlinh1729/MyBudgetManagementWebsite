using MyBudgetManagement.AppService.AuthSevice;

namespace MyBudgetManagement.Middlewares;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuthMiddleware> _logger;
    private readonly IConfiguration _configuration;

    public AuthMiddleware(RequestDelegate next, ILogger<AuthMiddleware> logger, IConfiguration configuration)
    {
        _next = next;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string token = "";
        string page403 = "/home/error";
        string pageLogin = "/api/user/login";
      
        try
        {
            _logger.LogInformation("Request received: {Method} {Path}", context.Request.Method, context.Request.Path);

            if (!context.GetEndpoint().Metadata.Where(x=>x.GetType().Name=="login").Any())
            {
                await _next(context);
                return;
            }    
            if (context.Request.Cookies.TryGetValue("JwtToken",out token))
            {
                var _jwtProvider = new JwtProvider(token);
                // Call the next middleware in the pipeline
                bool isValid = _jwtProvider.ValidateToken(token);
                if (isValid)
                {
                    await _next(context);
                    return;
                }
            }

            var pageRedirect = pageLogin;
            if (context.Request.Path.ToString()!=pageLogin)
            {
                pageLogin += "?UrlRedirect=" + context.Request.Path.ToString();
            }
            return;
        
        }
        catch (Exception e)
        {
            context.Response.Redirect("/login");
            return;
        }
        _logger.LogInformation("Response sent: {StatusCode}", context.Response.StatusCode);

    }
}