using MyBudgetManagement.AppService.AuthSevice;
using MyBudgetManagement.AppService.CategoryAppService;
using MyBudgetManagement.AppService.MD5Service;
using MyBudgetManagement.AppService.MD5Service.FileHandle;
using MyBudgetManagement.AppService.TransactionAppService;
using MyBudgetManagement.AppService.UserAppService;
using MyBudgetManagement.AppService.UserBalanceAppService;
using MyBudgetManagement.AppServices.AccountProfile;
using MyBudgetManagement.EntityFrameworkCore;
using MyBudgetManagement.Repositories;
using MyBudgetManagement.Repositories.AccountProfiles;
using MyBudgetManagement.Repositories.Applications;
using MyBudgetManagement.Repositories.Categories;

namespace MyBudgetManagement.Middlewares;

public static class UseToAddServiceExtensions
{
    public static void UseToAddAllScope(this IServiceCollection services)
    {
        services.AddScoped<IUserAppService, UserAppService>();
        services.AddScoped<IUserBalanceAppService, UserBalanceAppService>();
        services.AddScoped<IAccountProfileAppService, AccountProfileAppService>();
        services.AddScoped<IAccountProfileRepository, AccountProfileRepository>();
        services.AddScoped<ITransactionAppService, TransactionAppService>();
        services.AddScoped<ICategoryAppService, CategoryAppService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITransactionRepository, TransationRepository>();
        services.AddScoped<IUserBalanceRepository, UserBalanceRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IApplicationRepository, ApplicationRepository>();
        services.AddScoped<JwtProvider>();
        services.AddScoped<GetCurrentUser>();
        services.AddScoped<FileHandler>();
        services.AddScoped<DataSeeder>();
    }
}
