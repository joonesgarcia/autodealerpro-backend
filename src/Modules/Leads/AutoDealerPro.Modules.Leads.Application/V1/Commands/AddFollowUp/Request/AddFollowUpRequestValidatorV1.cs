using FluentValidation;

namespace AutoDealerPro.Modules.Leads.Application.V1.Commands.AddFollowUp.Request;

public class AddFollowUpRequestValidatorV1 : AbstractValidator<AddFollowUpRequestV1>
{
    public AddFollowUpRequestValidatorV1()
    {
        RuleFor(x => x.Note)
            .NotEmpty().WithMessage("Notes are required");

        RuleFor(x => x.NextFollowUpDate)
            .GreaterThan(DateTime.UtcNow).WithMessage("Next follow-up date must be in the future")
            .When(x => x.NextFollowUpDate.HasValue);
    }
}
