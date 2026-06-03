using FluentValidation;

namespace AutoDealerPro.Modules.Auth.Core.Requests.V1.Login;

public class LoginValidatorV1 : AbstractValidator<LoginRequestV1>
{
    public LoginValidatorV1()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(50);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(100);
    }
}
