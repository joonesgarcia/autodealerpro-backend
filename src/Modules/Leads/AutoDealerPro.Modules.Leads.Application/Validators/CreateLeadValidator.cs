using AutoDealerPro.Modules.Leads.Application.Requests;
using FluentValidation;

namespace AutoDealerPro.Modules.Leads.Application.Validators;

public class CreateLeadValidator : AbstractValidator<CreateLeadRequest>
{
    public CreateLeadValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(100).WithMessage("First name cannot exceed 100 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email format is invalid");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone is required")
            .Matches(@"^\+?[\d\s\-()]{10,}$").WithMessage("Phone format is invalid");

        RuleFor(x => x.VehicleId)
            .NotEmpty().WithMessage("Vehicle ID is required");

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Lead type is required")
            .Must(IsValidLeadType).WithMessage("Invalid lead type. Valid types: GeneralInquiry, TestDrive, TradeIn");

        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Message is required")
            .MaximumLength(1000).WithMessage("Message cannot exceed 1000 characters");

        RuleFor(x => x.TradeInYear)
            .InclusiveBetween(1900, DateTime.UtcNow.Year).WithMessage("Trade-in year must be between 1900 and current year")
            .When(x => x.TradeInYear.HasValue);

        RuleFor(x => x.TradeInMileage)
            .GreaterThanOrEqualTo(0).WithMessage("Trade-in mileage cannot be negative")
            .When(x => x.TradeInMileage.HasValue);
    }

    private static bool IsValidLeadType(string type)
    {
        return Enum.TryParse<AutoDealerPro.Modules.Leads.Core.Enums.LeadType>(type, true, out _);
    }
}
