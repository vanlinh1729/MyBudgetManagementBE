using FluentValidation;

namespace MyBudgetManagement.Application.Features.Users.Commands.UpdateUserProfile;

public class UpdateUserProfileCommandValidator : AbstractValidator<UpdateUserProfileCommand>
{
    public UpdateUserProfileCommandValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Họ tên là bắt buộc")
            .MinimumLength(2).WithMessage("Họ tên phải có ít nhất 2 ký tự")
            .MaximumLength(100).WithMessage("Họ tên không được quá 100 ký tự")
            .Matches(@"^[a-zA-ZÀ-ỹ\s]+$").WithMessage("Họ tên chỉ được chứa chữ cái và khoảng trắng");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Ngày sinh là bắt buộc")
            .Must(BeValidAge).WithMessage("Tuổi phải từ 13 đến 100");

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^(\+84|0)[0-9]{9,10}$")
            .WithMessage("Số điện thoại không hợp lệ (VD: 0912345678 hoặc +84912345678)")
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

        RuleFor(x => x.Gender)
            .IsInEnum().WithMessage("Giới tính không hợp lệ");

        RuleFor(x => x.Currency)
            .IsInEnum().WithMessage("Loại tiền tệ không hợp lệ");
    }

    private static bool BeValidAge(DateTime dateOfBirth)
    {
        var age = DateTime.Now.Year - dateOfBirth.Year;
        if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
            age--;
        
        return age >= 13 && age <= 100;
    }
} 