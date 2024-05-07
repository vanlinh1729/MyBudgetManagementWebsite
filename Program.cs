using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyBudgetManagement.AppService.UserAppService;
using MyBudgetManagement.EntityFrameworkCore;
using MyBudgetManagement.Middlewares;
using MyBudgetManagement.Repositories;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Sinks.File;

Log.Logger = new LoggerConfiguration()
#if DEBUG
    .MinimumLevel.Debug()
#else
            .MinimumLevel.Information()
#endif
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Sink(new FileSink("Logs/logs.txt", new JsonFormatter(), long.MaxValue, Encoding.Default))
    .WriteTo.Console()
    .CreateLogger();

try
{
    Log.Information("Starting web host.");
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddControllersWithViews();
    builder.Services.AddSwaggerGen(x =>
        x.SwaggerDoc("v1", new OpenApiInfo { Title = "MyBudgetManagementAPI", Version = "v1" }));
    builder.Services.AddHttpContextAccessor();

    builder.Services.AddAutoMapper(typeof(Program));

    builder.Services.UseToAddAllScope();

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("SQLConnectionString")));

    //Add Jwt bearer authentication
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration.GetSection("Jwt")["Issuer"],
                ValidAudience = builder.Configuration.GetSection("Jwt")["Audience"],
                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt")["SecretKey"]))
            };
        });



    builder.Host
        .UseSerilog();

    var app = builder.Build();
    // Retrieve the instance of DataSeeder from the service provider
    var scope = app.Services.CreateScope();
    var dataSeeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    dataSeeder.Seed(); // Seed data
    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyBudgetManagementAPI v1");
        c.RoutePrefix = "swagger";
    });

    app.UseRouting();
    app.UseAuthentication();
    app.UseMiddleware<ValidateTokenMiddleware>();
    app.UseAuthorization();
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllerRoute("default",
            "{controller=Login}/{action=Index}");
    });

    await app.RunAsync();

    return 0;
}
catch (Exception ex)
{
    if (ex is HostAbortedException) throw;

    Log.Fatal(ex, "Host terminated unexpectedly!");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}