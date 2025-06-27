using FluentValidation;

namespace MyBudgetManagement.Application.Features.Auth.Commands.RegisterUser;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email là bắt buộc")
            .EmailAddress().WithMessage("Email không hợp lệ")
            .MaximumLength(255).WithMessage("Email không được quá 255 ký tự");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Mật khẩu là bắt buộc")
            .MinimumLength(6).WithMessage("Mật khẩu phải có ít nhất 6 ký tự")
            .MaximumLength(100).WithMessage("Mật khẩu không được quá 100 ký tự")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$")
            .WithMessage("Mật khẩu phải có ít nhất 1 chữ thường, 1 chữ hoa và 1 số");

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Họ tên là bắt buộc")
            .MinimumLength(2).WithMessage("Họ tên phải có ít nhất 2 ký tự")
            .MaximumLength(100).WithMessage("Họ tên không được quá 100 ký tự")
            .Matches(@"^[a-zA-ZÀ-ỹ\s]+$").WithMessage("Họ tên chỉ được chứa chữ cái và khoảng trắng");
    }
} 