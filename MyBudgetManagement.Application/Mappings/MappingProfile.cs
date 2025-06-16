using AutoMapper;
using MyBudgetManagement.Application.DTOs;
using MyBudgetManagement.Application.Features.Categories.Dtos;
using MyBudgetManagement.Application.Features.Transactions.Dtos;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Entities.Categories;
using MyBudgetManagement.Domain.Entities.Transactions;
using MyBudgetManagement.Domain.Entities.Users;

namespace MyBudgetManagement.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // CreateMap<CreateUserCommand, User>();
        // CreateMap<CreateUserCommand, CreateUserDto>();
        // CreateMap<CreateUserDto, User>();
        // CreateMap<CreateAccountProfileCommand, AccountProfile>();
        // CreateMap<UpdateAccountProfileCommand, AccountProfile>();
        // CreateMap<AccountProfile, AccountProfileDto>();
        // CreateMap<User, UserDto>()
        //     .ForMember(dest => dest.UserRoles, 
        //         opt => opt.MapFrom(src => src.UserRoles
        //             .Select(ur => new UserRoleDto 
        //             { 
        //                 RoleId = ur.RoleId, 
        //                 RoleName = ur.Role.Name 
        //             }).ToList()));
        //
        // CreateMap<UserRole, UserRoleDto>();    
        CreateMap<UserBalance, UserBalanceDto>();
        CreateMap<Category, CategoryDto>();
        CreateMap<Transaction, TransactionDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.CategoryType, opt => opt.MapFrom(src => src.Category.Type));
        
    }
}