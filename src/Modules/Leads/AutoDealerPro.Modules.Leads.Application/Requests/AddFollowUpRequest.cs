namespace AutoDealerPro.Modules.Leads.Application.Requests;

public record AddFollowUpRequest(
    string Notes,
    DateTime? NextFollowUpDate = null
);
