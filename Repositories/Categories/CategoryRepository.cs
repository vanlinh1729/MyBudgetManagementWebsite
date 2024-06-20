using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.AppService.AuthSevice;
using MyBudgetManagement.EntityFrameworkCore;
using MyBudgetManagement.Models.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBudgetManagement.Repositories.Categories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JwtProvider _jwtProvider;

        public CategoryRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, JwtProvider jwtProvider)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _jwtProvider = jwtProvider;
        }

        public async Task<List<Category>> GetListCategoriesAsync()
        {
            string token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"]
                .FirstOrDefault()?.Split(" ").Last();

            var currentUserId = _jwtProvider.GetUserIdFromJwtToken(token);
            
            var categories = await _context.Category
                .Where(c => c.UserId == currentUserId)
                .ToListAsync();

            return categories;
        }

        public async Task<Category> GetCategoryByIdAsync(Guid id)
        {
            return await _context.Category.FirstOrDefaultAsync(x=>x.Id == id);
        }

        public async Task<int> CreateCategoriesAsync(Category category)
        {
            try
            {
                _context.Category.Add(category);
                return _context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<decimal> GetBalanceAsync(Guid userbalanceid)
        {
            return await _context.UserBalances
                .Where(x => x.Id == userbalanceid)
                .Select(x => x.Balance)
                .FirstOrDefaultAsync();
        }
        public async Task<decimal> GetSpentAsync(Guid categoryid)
        {
            return await _context.Transactions.Where(x=>x.CategoryId == categoryid).Select(x=>x.Amount).SumAsync();
        }
    }
}
