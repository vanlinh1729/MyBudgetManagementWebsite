using AutoMapper;
using AutoMapper.Internal.Mappers;
using MyBudgetManagement.Dtos;
using MyBudgetManagement.Dtos.AccountProfiles;
using MyBudgetManagement.Dtos.Categories;
using MyBudgetManagement.Dtos.Transactions;
using MyBudgetManagement.Dtos.UserBalances;
using MyBudgetManagement.Dtos.Users;
using MyBudgetManagement.Models;
using MyBudgetManagement.Models.Categories;
using MyBudgetManagement.ViewModels.Login;
using MyBudgetManagement.ViewModels.Transaction;
using MyBudgetManagement.ViewModels.UserBalances;

namespace MyBudgetManagement.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<CategoryDto, SelectedDto>();
        CreateMap<UserDto, User>();
        CreateMap<RegisterViewModel, User>();
        CreateMap<UserBalance, UserBalanceDto>();
        CreateMap<AccountProfile, AccountProFileViewDto>();
        CreateMap<Category, CategoryDto>();
        CreateMap<Transaction, TransactionDto>();
        CreateMap<Task<Transaction>, Task<TransactionDto>>();
        CreateMap<CreateOrUpdateUserBalanceViewModel, UserBalance>();
        CreateMap<CreateOrUpdateTransactionViewModel , Transaction>();

    }
}