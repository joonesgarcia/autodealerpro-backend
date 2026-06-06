namespace AutoDealerPro.Modules.Leads.Application.Requests.V1.AddFollowUp;

public record AddFollowUpRequestV1(
    string Note,
    DateTime? NextFollowUpDate = null
);
