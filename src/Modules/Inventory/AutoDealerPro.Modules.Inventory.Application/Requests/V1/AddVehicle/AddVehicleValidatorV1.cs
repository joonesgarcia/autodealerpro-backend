using FluentValidation;

namespace AutoDealerPro.Modules.Inventory.Application.Requests.V1.AddVehicle;

public class AddVehicleValidatorV1 : AbstractValidator<AddVehicleRequestV1>
{
    public AddVehicleValidatorV1()
    {
        RuleFor(x => x.Make).NotEmpty().WithMessage("Make is required");
        RuleFor(x => x.Model).NotEmpty().WithMessage("Model is required");
        RuleFor(x => x.Year).NotEmpty().WithMessage("Year is required");
        RuleFor(x => x.PlateNumber).NotEmpty().WithMessage("Plate number is required");
        RuleFor(x => x.Trim).NotEmpty().WithMessage("Trim is required");
        RuleFor(x => x.Mileage).NotEmpty().WithMessage("Mileage is required");
        RuleFor(x => x.ExteriorColor).NotEmpty().WithMessage("Exterior color is required");
        RuleFor(x => x.InteriorColor).NotEmpty().WithMessage("Interior color is required");
        RuleFor(x => x.Transmission).NotEmpty().WithMessage("Transmission is required");
        RuleFor(x => x.FuelType).NotEmpty().WithMessage("Fuel type is required");
        RuleFor(x => x.BodyType).NotEmpty().WithMessage("Body type is required");
        RuleFor(x => x.PurchasePrice).NotEmpty().WithMessage("Purchase price is required");
        RuleFor(x => x.AskingPrice).NotEmpty().WithMessage("Asking price is required");
    }
}
