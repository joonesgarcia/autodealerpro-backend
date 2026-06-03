using FluentValidation;

namespace AutoDealerPro.Modules.Leads.Application.Requests.V1.AddFollowUp;

public class AddFollowUpValidatorV1 : AbstractValidator<AddFollowUpRequestV1>
{
    public AddFollowUpValidatorV1()
    {
        RuleFor(x => x.Notes)
            .NotEmpty().WithMessage("Notes are required")
            .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters");

        RuleFor(x => x.NextFollowUpDate)
            .GreaterThan(DateTime.UtcNow).WithMessage("Next follow-up date must be in the future")
            .When(x => x.NextFollowUpDate.HasValue);
    }
}
