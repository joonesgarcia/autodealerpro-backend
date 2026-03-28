using AutoDealerPro.Modules.Leads.Application.Requests;
using FluentValidation;

namespace AutoDealerPro.Modules.Leads.Application.Validators;

public class AssignLeadValidator : AbstractValidator<AssignLeadRequest>
{
    public AssignLeadValidator()
    {
        RuleFor(x => x.StaffId)
            .NotEmpty().WithMessage("Staff ID is required");
    }
}
