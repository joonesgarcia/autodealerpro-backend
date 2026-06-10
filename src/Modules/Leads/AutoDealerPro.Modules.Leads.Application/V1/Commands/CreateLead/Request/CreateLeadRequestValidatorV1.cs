using FluentValidation;

namespace AutoDealerPro.Modules.Leads.Application.V1.Commands.CreateLead.Request;

public class CreateLeadRequestValidatorV1 : AbstractValidator<CreateLeadRequestV1>
{
    public CreateLeadRequestValidatorV1()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone is required");

        RuleFor(x => x.VehicleId)
            .NotEmpty().WithMessage("Vehicle ID is required");

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Lead type is required");

        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Message is required");

        RuleFor(x => x.TradeInMileage)
            .GreaterThanOrEqualTo(0).WithMessage("Trade-in mileage cannot be negative")
            .When(x => x.TradeInMileage.HasValue);
    }
}
