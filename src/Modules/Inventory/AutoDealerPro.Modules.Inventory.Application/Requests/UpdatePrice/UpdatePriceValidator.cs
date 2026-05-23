using FluentValidation;

namespace AutoDealerPro.Modules.Inventory.Application.Requests.UpdatePrice;

public class UpdatePriceValidator : AbstractValidator<UpdatePriceRequest>
{
    public UpdatePriceValidator()
    {
        RuleFor(x => x.NewPrice).GreaterThan(0);
    }
}
