using FluentValidation;

namespace AutoDealerPro.Modules.Inventory.Application.Requests.MarkAsSold;

public class MarkAsSoldValidator : AbstractValidator<MarkAsSoldRequest>
{
    public MarkAsSoldValidator()
    {
        RuleFor(x => x.SellingPrice).GreaterThan(0);
    }
}
