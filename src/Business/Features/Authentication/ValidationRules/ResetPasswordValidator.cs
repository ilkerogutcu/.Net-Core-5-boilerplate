#region

using Business.Constants;
using Business.Features.Authentication.Commands;
using FluentValidation;

#endregion

namespace Business.Features.Authentication.ValidationRules
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordValidator()
        {
            RuleFor(x => x.Password).Equal(x => x.ConfirmPassword)
                .WithMessage(Messages.PasswordsDontMatch);
        }
    }
}