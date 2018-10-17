using FluentValidation;
using PortableLibrary.Core.Infrastructure.Models;

namespace PortableLibrary.Core.Membership.Validation
{
    public class RegisterModelValidator : AbstractValidator<RegisterModel>
    {
        public RegisterModelValidator()
        {
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Password).Length(6, 12);
            RuleFor(x => x.ConfirmPassword).Matches(x => x.Password);
        }
    }
}
