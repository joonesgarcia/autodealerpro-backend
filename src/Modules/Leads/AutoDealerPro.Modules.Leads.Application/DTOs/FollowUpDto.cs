namespace AutoDealerPro.Modules.Leads.Application.DTOs;

public record FollowUpDto(
    Guid Id,
    string Notes,
    DateTime CreatedAt,
    DateTime? NextFollowUpDate
);
