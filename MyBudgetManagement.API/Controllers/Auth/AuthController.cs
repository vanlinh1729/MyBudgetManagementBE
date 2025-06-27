using System.Data.Entity;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Features.Auth.Commands.ActivateAccount;
using MyBudgetManagement.Application.Features.Auth.Commands.Login;
using MyBudgetManagement.Application.Features.Auth.Commands.RegisterUser;
using MyBudgetManagement.Application.Features.Auth.Commands.ResendActivationEmail;
using MyBudgetManagement.Application.Features.Auth.Commands.RefreshToken;
using MyBudgetManagement.Application.Features.Auth.Commands.Logout;
using MyBudgetManagement.Application.Features.Auth.Dtos;
using MyBudgetManagement.Application.Features.Auth.Interfaces;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.API.Controllers.Auths;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserCommand command)
    {
        var userId = await _mediator.Send(command);
        return Ok(new { UserId = userId });
    }
    [HttpGet("activate")]
    public async Task<IActionResult> ActivateAccount([FromQuery] string token)
    {
        await _mediator.Send(new ActivateAccountCommand { Token = token });
        return Ok("Tài khoản đã được kích hoạt thành công.");
    }
    
    [HttpGet("resend-activation-email")]
    public async Task<IActionResult> ResendActivationEmail([FromQuery] string email)
    {
        await _mediator.Send(new ResendActivationEmailCommand { Email = email });
        return Ok("Đã gửi lại email kích hoạt tài khoản, vui lòng kiểm tra lại hộp thư.");
    }
    
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginCommand command)
    {
        var authResponse = await _mediator.Send(command);
        return Ok(authResponse);
    }

    [AllowAnonymous]
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(RefreshTokenCommand command)
    {
        var authResponse = await _mediator.Send(command);
        return Ok(authResponse);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout(LogoutCommand command)
    {
        await _mediator.Send(command);
        return Ok(new { message = "Đăng xuất thành công" });
    }

}