using FluentValidation;

namespace AutoDealerPro.Modules.Inventory.Application.Validators;

public class MarkAsSoldValidator : AbstractValidator<Requests.MarkAsSoldRequest>
{
    public MarkAsSoldValidator()
    {
        RuleFor(x => x.SellingPrice).GreaterThan(0);
    }
}
