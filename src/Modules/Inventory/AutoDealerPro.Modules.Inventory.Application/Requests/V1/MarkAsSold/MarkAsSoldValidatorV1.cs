using FluentValidation;

namespace AutoDealerPro.Modules.Inventory.Application.Requests.V1.MarkAsSold;

public class MarkAsSoldValidatorV1 : AbstractValidator<MarkAsSoldRequestV1>
{
    public MarkAsSoldValidatorV1()
    {
        RuleFor(x => x.SellingPrice).GreaterThan(0);
    }
}
