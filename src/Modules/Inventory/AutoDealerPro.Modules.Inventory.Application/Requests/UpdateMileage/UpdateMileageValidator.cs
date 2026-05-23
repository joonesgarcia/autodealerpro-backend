using FluentValidation;

namespace AutoDealerPro.Modules.Inventory.Application.Requests.UpdateMileage;

public class UpdateMileageValidator : AbstractValidator<UpdateMileageRequest>
{
    public UpdateMileageValidator()
    {
        RuleFor(x => x.NewMileage).GreaterThanOrEqualTo(0);
    }
}
