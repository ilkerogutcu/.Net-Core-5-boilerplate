#region

using Business.Constants;
using Business.Features.Authentication.Commands;
using FluentValidation;

#endregion

namespace Business.Features.Authentication.ValidationRules
{
    public class SignUpValidator : AbstractValidator<SignUpUserCommand>
    {
        public SignUpValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage(Messages.PleaseEnterTheEmail);
            RuleFor(x => x.Email).EmailAddress().WithMessage(Messages.PleaseEnterAValidEmail);
            RuleFor(x => x.FirstName).NotEmpty().WithMessage(Messages.PleaseEnterTheFirstName);
            RuleFor(x => x.LastName).NotEmpty().WithMessage(Messages.PleaseEnterTheLastName);
            RuleFor(x => x.Password).Equal(x => x.ConfirmPassword).WithMessage(Messages.PasswordsDontMatch);
        }
    }
}