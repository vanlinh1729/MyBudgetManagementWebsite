using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.EntityFrameworkCore;
using MyBudgetManagement.Models;

namespace MyBudgetManagement.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context = null;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User> CreateOrUpdateUser(User userModel)
    {
        try
        {
            var checkUser = await GetUserByEmail(userModel.Email);
            if (checkUser.Email != userModel.Email)
            {
                var user = _context.Users
                    .FromSqlRaw("EXEC CreateUser @p0, @p1, @p2, @p3, @p4",userModel.Id.ToString(), userModel.FirstName, userModel.LastName, userModel.Email, userModel.Password)
                    .AsEnumerable().First();
                var userRole = _context.Roles.First(x => x.Name == Shared.Users.Role.User);
                var roleId = _context.UserRoles.First(x => x.Role == userRole).RoleId;
                _context.Database.ExecuteSqlRaw("EXEC SetRoleUserForNewAccount @p0, @p1, @p2", Guid.NewGuid(), user.Id,roleId);
                return user;
            }
           
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return new User();
        }

        return new User();
    }

    public Task<string> DeleteUser(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<User>> GetAllUsers()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> GetUserById(Guid id)
    {
        var user = await _context.Users.Where(x=>x.Id == id).FirstOrDefaultAsync();
        return user;
    }

    public async Task<User> GetUserByEmail(string email)
    {
        try
        {
            var users = _context.Users
                .FromSqlRaw("EXEC GetUserByEmail @p0", email)
                .AsEnumerable().ToList();

            return users.Count != 0 ? users.First() : new User();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
       
    }

    public async Task<bool> CheckPasswordAsync(string email, string password)
    {
        var user = await GetUserByEmail(email);
        if (user != null && user.Password == password)
            return true;
        return false;
    }

    public List<Role> GetUserRoleByUserId(Guid id)
    {
        var listRoles = _context.Roles.FromSqlRaw("EXEC GetRoleByUserId @p0", id).AsEnumerable();
        return listRoles.ToList();
    }
}