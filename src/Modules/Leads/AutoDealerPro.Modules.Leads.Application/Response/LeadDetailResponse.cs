namespace AutoDealerPro.Modules.Leads.Application.Response;

public record LeadDetailResponse(
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
    IList<FollowUpResponse> FollowUps,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
