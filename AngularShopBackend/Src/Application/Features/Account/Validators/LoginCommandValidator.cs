using Application.Features.Account.Commands.LoginUser;
using FluentValidation;

namespace Application.Features.Account.Validators;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("وارد کردن شماره همراه الزامی است")
            .Matches(@"^\d{11}$").WithMessage("شماره همراه باید شامل 11 رقم باشد");
        
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("وارد کردن رمز ورود الزامی است")
            .MinimumLength(6).WithMessage("رمز عبور باید حداقل ۶ کاراکتر باشد.");
    }
}
