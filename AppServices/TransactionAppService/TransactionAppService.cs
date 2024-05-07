using AutoMapper;
using MyBudgetManagement.AppService.AuthSevice;
using MyBudgetManagement.Dtos.Transactions;
using MyBudgetManagement.Models;
using MyBudgetManagement.Repositories;

namespace MyBudgetManagement.AppService.TransactionAppService;

public class TransactionAppService : ITransactionAppService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly JwtProvider _jwtProvider;
    private readonly IMapper _mapper;


    public TransactionAppService(ITransactionRepository transactionRepository, IHttpContextAccessor httpContextAccessor, JwtProvider jwtProvider, IMapper mapper)
    {
        _transactionRepository = transactionRepository;
        _httpContextAccessor = httpContextAccessor;
        _jwtProvider = jwtProvider;
        _mapper = mapper;
    }

    public async Task<List<TransactionDto>> GetListTransactionAsync()
    {
        var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (token != null)
        {
            var userId = _jwtProvider.GetUserIdFromJwtToken(token);
            var userEmail = _jwtProvider.GetUserEmailFromJwtToken(token);

            if (userId != Guid.Empty)
            {
                var listTransactions = _transactionRepository.GetAllTransaction(userId).Result;
                var listDtos = new List<TransactionDto>();
                foreach (var transaction in listTransactions)
                {

                    var transactionDto = new TransactionDto
                    {
                        Id = transaction.Id,
                        UserId = transaction.UserId,
                        Email = userEmail, // Assuming User is a property in Transaction class
                        Title = transaction.Title,
                        CategoryId = transaction.Category != null ? transaction.Category.Id : null,
                        CategoryName = transaction.Category != null ? transaction.Category.Name : null,
                        Amount = transaction.Amount,
                        Description = transaction.Description,
                        Type = transaction.Type,
                        Date = transaction.Date
                    };
                    listDtos.Add(transactionDto);
                }
                return listDtos;
            }
        }
        return new List<TransactionDto>();
    }

    public async Task<int> CreateTransaction(Transaction model)
    {
        var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ")
            .Last();
        if (token != null)
        {
            var userId = _jwtProvider.GetUserIdFromJwtToken(token);
            var userEmail = _jwtProvider.GetUserEmailFromJwtToken(token);
            if (userId != Guid.Empty)
            {
                model.Id = Guid.NewGuid();
                model.UserId = userId;
                model.CategoryId = model.CategoryId;
                var transaction = await _transactionRepository.CreateOrUpdateTransactionAndUpdateUserBalance(model);
                return transaction;
            }
        }

        // Trả về null hoặc throw một ngoại lệ tùy thuộc vào logic của bạn
        throw new Exception("Unauthorized");
    }
}