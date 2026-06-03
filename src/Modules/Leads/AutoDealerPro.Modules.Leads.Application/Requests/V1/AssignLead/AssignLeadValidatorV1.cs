using FluentValidation;

namespace AutoDealerPro.Modules.Leads.Application.Requests.V1.AssignLead;

public class AssignLeadValidatorV1 : AbstractValidator<AssignLeadRequestV1>
{
    public AssignLeadValidatorV1()
    {
        RuleFor(x => x.StaffId)
            .NotEmpty().WithMessage("Staff ID is required");
    }
}
