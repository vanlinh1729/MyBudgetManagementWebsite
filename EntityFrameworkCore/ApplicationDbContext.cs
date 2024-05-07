using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Models;
using MyBudgetManagement.Models.Categories;

namespace MyBudgetManagement.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Category> Category { get; set; }
    public DbSet<UserBalance> UserBalances { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
}