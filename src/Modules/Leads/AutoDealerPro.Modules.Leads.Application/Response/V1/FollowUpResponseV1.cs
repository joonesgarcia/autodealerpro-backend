namespace AutoDealerPro.Modules.Leads.Application.Response.V1;

public record FollowUpResponseV1(
    Guid Id,
    string Notes,
    DateTime CreatedAt,
    DateTime? NextFollowUpDate
);
