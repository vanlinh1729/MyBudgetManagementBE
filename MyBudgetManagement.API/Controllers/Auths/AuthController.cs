using System.Data.Entity;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Features.Auth.Commands.ActivateAccount;
using MyBudgetManagement.Application.Features.Auth.Commands.RegisterUser;
using MyBudgetManagement.Application.Features.Auth.Dtos;
using MyBudgetManagement.Application.Features.Auth.Interfaces;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.API.Controllers;

[ApiController]
[Route("api/auths")]
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

}