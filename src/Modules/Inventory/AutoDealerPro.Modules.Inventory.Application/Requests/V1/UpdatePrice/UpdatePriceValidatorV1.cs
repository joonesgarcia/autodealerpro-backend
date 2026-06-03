using FluentValidation;

namespace AutoDealerPro.Modules.Inventory.Application.Requests.V1.UpdatePrice;

public class UpdatePriceValidatorV1 : AbstractValidator<UpdatePriceRequestV1>
{
    public UpdatePriceValidatorV1()
    {
        RuleFor(x => x.NewPrice).GreaterThan(0);
    }
}
