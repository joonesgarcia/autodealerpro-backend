namespace AutoDealerPro.Modules.Leads.Application.V1.Response;

public record FollowUpResponseV1(
    Guid Id,
    string Notes,
    DateTime CreatedAt,
    DateTime? NextFollowUpDate
);
