using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBudgetManagement.Application.Features.Users.Commands.ChangePassword;
using MyBudgetManagement.Application.Features.Users.Commands.DeleteAccount;
using MyBudgetManagement.Application.Features.Users.Commands.UpdateUserProfile;
using MyBudgetManagement.Application.Features.Users.Dtos;
using MyBudgetManagement.Application.Features.Users.Queries.GetUserProfile;

namespace MyBudgetManagement.API.Controllers;

[Route("api/users")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    /// <summary>
    /// Get current user profile
    /// </summary>
    [HttpGet("profile")]
    public async Task<IActionResult> GetMyProfile()
    {
        var userId = GetCurrentUserId();
        var query = new GetUserProfileQuery { UserId = userId };
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    /// <summary>
    /// Get user profile by ID (for admin or public profiles)
    /// </summary>
    [HttpGet("{id:guid}/profile")]
    public async Task<IActionResult> GetUserProfile(Guid id)
    {
        var currentUserId = GetCurrentUserId();
        
        // Users can only view their own profile, unless they're admin
        if (currentUserId != id && !User.IsInRole("Admin"))
        {
            return Forbid("Bạn chỉ có thể xem profile của chính mình");
        }
        
        var query = new GetUserProfileQuery { UserId = id };
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    /// <summary>
    /// Update current user profile
    /// </summary>
    [HttpPut("profile")]
    public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateUserProfileDto dto)
    {
        var userId = GetCurrentUserId();
        var command = new UpdateUserProfileCommand
        {
            UserId = userId,
            FullName = dto.FullName,
            Gender = dto.Gender,
            DateOfBirth = dto.DateOfBirth,
            PhoneNumber = dto.PhoneNumber,
            Currency = dto.Currency
        };
        
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    /// <summary>
    /// Change password
    /// </summary>
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        var userId = GetCurrentUserId();
        var command = new ChangePasswordCommand
        {
            UserId = userId,
            CurrentPassword = dto.CurrentPassword,
            NewPassword = dto.NewPassword,
            ConfirmPassword = dto.ConfirmPassword
        };
        
        await _mediator.Send(command);
        return Ok(new { message = "Đổi mật khẩu thành công" });
    }
    
    /// <summary>
    /// Delete current user account
    /// </summary>
    [HttpDelete("account")]
    public async Task<IActionResult> DeleteMyAccount([FromBody] DeleteAccountRequest request)
    {
        var userId = GetCurrentUserId();
        var command = new DeleteAccountCommand
        {
            UserId = userId,
            Password = request.Password,
            ConfirmationText = request.ConfirmationText
        };
        
        await _mediator.Send(command);
        return Ok(new { message = "Tài khoản đã được xóa thành công" });
    }
    
    /// <summary>
    /// Get current user ID from JWT token
    /// </summary>
    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                         ?? User.FindFirst("sub")?.Value
                         ?? User.FindFirst("userId")?.Value;
        
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("Không thể xác định user ID từ token");
        }
        
        return userId;
    }
}

/// <summary>
/// Request model for account deletion
/// </summary>
public class DeleteAccountRequest
{
    public string Password { get; set; } = string.Empty;
    public string ConfirmationText { get; set; } = string.Empty;
}