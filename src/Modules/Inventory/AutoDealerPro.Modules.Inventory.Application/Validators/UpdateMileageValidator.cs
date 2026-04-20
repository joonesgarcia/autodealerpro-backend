using FluentValidation;

namespace AutoDealerPro.Modules.Inventory.Application.Validators;

public class UpdateMileageValidator : AbstractValidator<Requests.UpdateMileageRequest>
{
    public UpdateMileageValidator()
    {
        RuleFor(x => x.NewMileage).GreaterThanOrEqualTo(0);
    }
}
