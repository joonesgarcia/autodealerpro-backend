using FluentValidation;

namespace AutoDealerPro.Modules.Inventory.Application.Validators;

public class CreateVehicleValidator : AbstractValidator<Requests.CreateVehicleRequest>
{
    public CreateVehicleValidator()
    {
        RuleFor(x => x.Make).NotEmpty();
        RuleFor(x => x.Model).NotEmpty();
        RuleFor(x => x.Year).InclusiveBetween(1900, DateTime.Now.Year + 1);
        RuleFor(x => x.PlateNumber).NotEmpty();
        RuleFor(x => x.Trim).NotEmpty();
        RuleFor(x => x.Mileage).GreaterThanOrEqualTo(0);
        RuleFor(x => x.ExteriorColor).NotEmpty();
        RuleFor(x => x.InteriorColor).NotEmpty();
        RuleFor(x => x.Transmission).NotEmpty();
        RuleFor(x => x.FuelType).NotEmpty();
        RuleFor(x => x.BodyType).NotEmpty();
        RuleFor(x => x.PurchasePrice).GreaterThan(0);
        RuleFor(x => x.AskingPrice).GreaterThan(0);
    }
}
