using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyBudgetManagement.AppService.MD5Service;
using MyBudgetManagement.Models;

namespace MyBudgetManagement.EntityFrameworkCore;

public class DataSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public DataSeeder(ApplicationDbContext context,IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public void Seed()
    {
        /*
        StoredProcedureDataSeed();
        */
        UserDataSeed();
        RoleDataSeed();
        UserRoleDataSeed();
    }

    public void UserDataSeed()
    {
        if (!_context.Users.Any())
        {
            var listMembers = new List<User>
            {
                new() { Id = Guid.NewGuid(), FirstName = "Linh", LastName = "Nguyen", Email = "admin@gmail.com", Password = Md5Encrypt.Encrypt("123") },
                new() { Id = Guid.NewGuid(), FirstName = "Linh2", LastName = "Nguyen", Email = "user@gmail.com", Password = Md5Encrypt.Encrypt("123") },
            };
            foreach (var member in listMembers) _context.Users.Add(member);
            _context.SaveChanges();
        }
    }

    public void RoleDataSeed()
    {
        if (!_context.Roles.Any())
        {
            var listRoles = new List<Role>
            {
                new() { Id = Guid.NewGuid(), Name = Shared.Users.Role.Admin },
                new() { Id = Guid.NewGuid(), Name = Shared.Users.Role.User }
            };
            foreach (var role in listRoles) _context.Roles.Add(role);

            _context.SaveChanges();
        }
    }

    public void UserRoleDataSeed()
    {
        if (!_context.UserRoles.Any() && _context.Users.Any() && _context.Roles.Any())
        {
            var adminRole = _context.Roles.Single(r => r.Name == Shared.Users.Role.Admin);
            var userRole = _context.Roles.Single(r => r.Name == Shared.Users.Role.User);
           

            var users = _context.Users.ToList();
            var roles = _context.Roles.ToList();
            foreach (var user in users)
                if (user.Email == "admin@gmail.com")
                {
                    _context.UserRoles.Add(new UserRole()
                        { Id = Guid.NewGuid(), UserId = user.Id, RoleId = adminRole.Id });
                    _context.UserRoles.Add(new UserRole()
                        { Id = Guid.NewGuid(), UserId = user.Id, RoleId = userRole.Id });
                } 
                else if (user.Email == "user@gmail.com")
                {
                   
                    _context.UserRoles.Add(new UserRole()
                        { Id = Guid.NewGuid(), UserId = user.Id, RoleId = userRole.Id });
                }

            _context.SaveChanges();
        }
    }

}