using FluentValidation;
using PortableLibrary.Core.Membership.Models;

namespace PortableLibrary.Core.Membership.Validation
{
    public class LoginModelValidator : AbstractValidator<LoginModel>
    {
        public LoginModelValidator()
        {
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Password).Length(6, 12);
        }
    }
}
