using AutoMapper;
using MyBudgetManagement.Application.DTOs;
using MyBudgetManagement.Domain.Entities;
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
        
    }
}