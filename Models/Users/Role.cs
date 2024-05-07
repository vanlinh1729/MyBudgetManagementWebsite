using System.Collections;

namespace MyBudgetManagement.Models;

public class Role
{
    public Guid Id { get; set; }
    public Shared.Users.Role Name { get; set; }
    public ICollection<UserRole> UserRoles { get; set; }
}