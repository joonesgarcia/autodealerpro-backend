namespace AutoDealerPro.Modules.Leads.Core.DTOs;

public record FollowUpDto(
    Guid Id,
    string Notes,
    DateTime CreatedAt,
    DateTime? NextFollowUpDate
);

public record LeadDetailDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string Phone,
    Guid VehicleId,
    string Type,
    string Status,
    string Message,
    string? TradeInMake,
    string? TradeInModel,
    int? TradeInYear,
    int? TradeInMileage,
    Guid? AssignedToStaffId,
    DateTime? ContactedAt,
    string? StaffNotes,
    List<FollowUpDto> FollowUps,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);