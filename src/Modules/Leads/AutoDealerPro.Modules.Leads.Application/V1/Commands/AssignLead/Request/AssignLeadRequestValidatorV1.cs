using FluentValidation;

namespace AutoDealerPro.Modules.Leads.Application.V1.Commands.AssignLead.Request;

public class AssignLeadRequestValidatorV1 : AbstractValidator<AssignLeadRequestV1>
{
    public AssignLeadRequestValidatorV1()
    {
        RuleFor(x => x.StaffId)
            .NotEmpty().WithMessage("Staff ID is required");
    }
}
