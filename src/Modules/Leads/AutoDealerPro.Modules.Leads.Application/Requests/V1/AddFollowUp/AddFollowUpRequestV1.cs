namespace AutoDealerPro.Modules.Leads.Application.Requests.V1.AddFollowUp;

public record AddFollowUpRequestV1(
    string Notes,
    DateTime? NextFollowUpDate = null
);
