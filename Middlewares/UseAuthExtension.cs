namespace MyBudgetManagement.Middlewares;

public static class UseAuthExtension
{
    public static IApplicationBuilder UseAuth(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);
        return app.UseMiddleware<AuthMiddleware>();
    }

}