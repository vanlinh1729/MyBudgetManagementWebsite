using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.EntityFrameworkCore;
using MyBudgetManagement.Models;

namespace MyBudgetManagement.Repositories;

public class UserBalanceRepository : IUserBalanceRepository
{
    private readonly ApplicationDbContext _context;

    public UserBalanceRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public UserBalance CreateOrUpdateUserBalance(UserBalance userBalance)
    {
        try
        {
            // Thực thi stored procedure để tạo hoặc cập nhật UserBalance
            _context.Database.ExecuteSqlRaw("EXEC CreateUserBalance @p0, @p1, @p2", 
                userBalance.Id, userBalance.UserId, userBalance.Balance);
        
            // Truy vấn UserBalance vừa được tạo hoặc cập nhật
            var createdUserBalance = _context.UserBalances.FirstOrDefault(u => u.UserId == userBalance.UserId);

            if (createdUserBalance == null)
            {
                // Nếu không tìm thấy UserBalance, throw ra một Exception để thông báo lỗi
                throw new Exception("Failed to create or update UserBalance.");
            }

            // Trả về UserBalance vừa được tạo hoặc cập nhật
            return createdUserBalance;
        }
        catch (Exception ex)
        {
            // Xử lý lỗi ở đây, ví dụ: ghi log, thông báo cho người dùng, vv.
            Console.WriteLine($"Error: {ex.Message}");
            // Hoặc bạn có thể throw lại exception để cho lớp gọi biết rằng có lỗi xảy ra
            return new UserBalance();
        }
    }


    public Task<UserBalance> GetUserBalanceById(Guid id)
    {
        throw new NotImplementedException();
    }

    public UserBalance GetUserBalanceByUserId(Guid userId)
    {
        try
        {
            return _context.UserBalances.FromSqlRaw("EXEC GetUserBalanceByUserId @p0", userId.ToString()).AsEnumerable().First();
        }
        catch (Exception e)
        {
            return new UserBalance();
        }
    }
}