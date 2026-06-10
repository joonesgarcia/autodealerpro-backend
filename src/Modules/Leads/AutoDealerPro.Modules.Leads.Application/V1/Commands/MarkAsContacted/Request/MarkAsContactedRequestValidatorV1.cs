using FluentValidation;

namespace AutoDealerPro.Modules.Leads.Application.V1.Commands.MarkAsContacted.Request;

public class MarkAsContactedRequestValidatorV1 : AbstractValidator<MarkAsContactedRequestV1>
{
    public MarkAsContactedRequestValidatorV1()
    {
        RuleFor(x => x.Notes)
            .NotEmpty().WithMessage("Notes are required");
    }
}
