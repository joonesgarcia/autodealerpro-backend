namespace AutoDealerPro.Modules.Leads.Application.V1.Commands.AddFollowUp.Request;

public record AddFollowUpRequestV1(
    string Note,
    DateTime? NextFollowUpDate = null
);
