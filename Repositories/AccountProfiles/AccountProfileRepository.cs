using MyBudgetManagement.EntityFrameworkCore;
using MyBudgetManagement.Models;

namespace MyBudgetManagement.Repositories.AccountProfiles;

public class AccountProfileRepository : IAccountProfileRepository

{
    private readonly ApplicationDbContext _context;

    public AccountProfileRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AccountProfile> CreateAccountProfile(AccountProfile accountProfile)
    {
        await _context.AccountProfiles.AddAsync(accountProfile);
        if (await _context.SaveChangesAsync() ==1)
        {
            return accountProfile;
        }
        else
        {
            return null;
        }
    }

    public async Task<AccountProfile> GetAccountProfileById(Guid id)
    {
        return await _context.AccountProfiles.FindAsync(id);
    }

    public async Task<AccountProfile> GetAccountProfileByUserId(Guid id)
    {
        var accountprofile = _context.AccountProfiles.Where(x=>x.UserId == id).FirstOrDefault();
        return accountprofile;
    }
}