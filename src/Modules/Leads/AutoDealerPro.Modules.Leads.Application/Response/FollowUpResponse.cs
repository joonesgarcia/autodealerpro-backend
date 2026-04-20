namespace AutoDealerPro.Modules.Leads.Application.Response;

public record FollowUpResponse(
    Guid Id,
    string Notes,
    DateTime CreatedAt,
    DateTime? NextFollowUpDate
);
