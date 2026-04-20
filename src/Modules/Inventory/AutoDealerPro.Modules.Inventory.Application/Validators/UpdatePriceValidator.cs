using FluentValidation;

namespace AutoDealerPro.Modules.Inventory.Application.Validators;

public class UpdatePriceValidator : AbstractValidator<Requests.UpdatePriceRequest>
{
    public UpdatePriceValidator()
    {
        RuleFor(x => x.NewPrice).GreaterThan(0);
    }
}
