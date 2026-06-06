using FluentValidation;

namespace AutoDealerPro.Modules.Leads.Application.Requests.V1.AddFollowUp;

public class AddFollowUpValidatorV1 : AbstractValidator<AddFollowUpRequestV1>
{
    public AddFollowUpValidatorV1()
    {
        RuleFor(x => x.Note)
            .NotEmpty().WithMessage("Notes are required");

        RuleFor(x => x.NextFollowUpDate)
            .GreaterThan(DateTime.UtcNow).WithMessage("Next follow-up date must be in the future")
            .When(x => x.NextFollowUpDate.HasValue);
    }
}
