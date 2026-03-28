using AutoDealerPro.Modules.Leads.Application.Requests;
using FluentValidation;

namespace AutoDealerPro.Modules.Leads.Application.Validators;

public class AddFollowUpValidator : AbstractValidator<AddFollowUpRequest>
{
    public AddFollowUpValidator()
    {
        RuleFor(x => x.Notes)
            .NotEmpty().WithMessage("Notes are required")
            .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters");

        RuleFor(x => x.NextFollowUpDate)
            .GreaterThan(DateTime.UtcNow).WithMessage("Next follow-up date must be in the future")
            .When(x => x.NextFollowUpDate.HasValue);
    }
}
