using MyBudgetManagement.Models.Categories;

namespace MyBudgetManagement.Models;

public class User
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public ICollection<Transaction> Transactions { get; set; }
    public UserBalance UserBalance { get; set; }
    public ICollection<UserRole> UserRoles { get; set; }
    public ICollection<Category>? Categories { get; set; }

}