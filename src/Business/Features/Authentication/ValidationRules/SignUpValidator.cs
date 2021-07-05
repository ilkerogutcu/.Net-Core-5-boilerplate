using Business.Constants;
using Business.Features.Authentication.Commands;
using FluentValidation;

namespace Business.Features.Authentication.ValidationRules
{
    /// <summary>
    ///     Validator for sign up
    /// </summary>
    public class SignUpValidator : AbstractValidator<SignUpUserCommand>
    {
        public SignUpValidator()
        {
            RuleFor(x => x.SignUpRequest.Email).NotEmpty().WithMessage(Messages.PleaseEnterTheEmail);
            RuleFor(x => x.SignUpRequest.Email).EmailAddress().WithMessage(Messages.PleaseEnterAValidEmail);
            RuleFor(x => x.SignUpRequest.FirstName).NotEmpty().WithMessage(Messages.PleaseEnterTheFirstName);
            RuleFor(x => x.SignUpRequest.LastName).NotEmpty().WithMessage(Messages.PleaseEnterTheLastName);
            RuleFor(x => x.SignUpRequest.Password).Equal(x => x.SignUpRequest.ConfirmPassword).WithMessage(Messages.PasswordsDontMatch);
        }
    }
}