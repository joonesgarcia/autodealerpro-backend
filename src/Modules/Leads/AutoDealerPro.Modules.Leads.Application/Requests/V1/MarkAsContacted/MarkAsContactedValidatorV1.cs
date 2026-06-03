using FluentValidation;

namespace AutoDealerPro.Modules.Leads.Application.Requests.V1.MarkAsContacted;

public class MarkAsContactedValidatorV1 : AbstractValidator<MarkAsContactedRequestV1>
{
    public MarkAsContactedValidatorV1()
    {
        RuleFor(x => x.Notes)
            .NotEmpty().WithMessage("Notes are required")
            .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters");
    }
}
