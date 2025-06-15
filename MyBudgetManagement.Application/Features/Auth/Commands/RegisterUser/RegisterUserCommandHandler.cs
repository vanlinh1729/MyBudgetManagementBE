using MediatR;
using MyBudgetManagement.Application.Common.Exceptions;
using MyBudgetManagement.Application.Features.Auth.Interfaces;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Entities.Users;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Application.Features.Auth.Commands.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Guid>
{
    private readonly IUnitOfWork _uow;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IEmailService _mailService;

    public RegisterUserCommandHandler(IUnitOfWork uow, IJwtTokenService jwtTokenService,
        IPasswordHasher passwordHasher, IEmailService mailService)
    {
        _uow = uow;
        _jwtTokenService = jwtTokenService;
        _passwordHasher = passwordHasher;
        _mailService = mailService;
    }

    public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        await _uow.BeginTransactionAsync();

        try
        {
            var checkUserExists = await _uow.Users.EmailExistsAsync(request.Email);
            if (checkUserExists )
            throw new ConflictException("Email already exists.");

        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Email = request.Email,
            FullName = request.FullName,
            Currency = Currencies.VND,
            Gender = Gender.Other,
            Status = AccountStatus.Pending,
            Created = DateTime.Now,
            CreatedBy = userId
        };

        user.PasswordHash = _passwordHasher.Hash(request.Password);

        await _uow.Users.AddAsync(user);

        var activationToken = _jwtTokenService.GenerateRandomStringToken();
        var token = new Token
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            TokenValue = activationToken,
            Type = TokenType.ActivationToken,
            CreatedAt = DateTime.UtcNow,
            ExpireAt = DateTime.UtcNow.AddDays(7)
        };
        await _uow.Tokens.AddAsync(token);
        
        await _uow.SaveChangesAsync();
        await _uow.CommitAsync();

       
        await _mailService.SendActivateEmailAsync(user.Email, user.FullName,token.TokenValue);

        return userId;
        }
        catch (Exception e)
        {
            await _uow.RollbackAsync();
            throw;
        }
        
    }
}