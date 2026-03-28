namespace AutoDealerPro.Modules.Leads.Application.DTOs;

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
    IList<FollowUpDto> FollowUps,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
