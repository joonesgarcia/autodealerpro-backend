using AutoDealerPro.Modules.Leads.Application.Requests;
using FluentValidation;

namespace AutoDealerPro.Modules.Leads.Application.Validators;

public class MarkAsContactedValidator : AbstractValidator<MarkAsContactedRequest>
{
    public MarkAsContactedValidator()
    {
        RuleFor(x => x.Notes)
            .NotEmpty().WithMessage("Notes are required")
            .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters");
    }
}
