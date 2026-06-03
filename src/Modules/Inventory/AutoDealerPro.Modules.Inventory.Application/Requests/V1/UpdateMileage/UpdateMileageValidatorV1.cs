using FluentValidation;

namespace AutoDealerPro.Modules.Inventory.Application.Requests.V1.UpdateMileage;

public class UpdateMileageValidatorV1 : AbstractValidator<UpdateMileageRequestV1>
{
    public UpdateMileageValidatorV1()
    {
        RuleFor(x => x.NewMileage).GreaterThanOrEqualTo(0);
    }
}
