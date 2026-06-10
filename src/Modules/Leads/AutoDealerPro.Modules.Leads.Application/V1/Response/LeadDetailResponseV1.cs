namespace AutoDealerPro.Modules.Leads.Application.V1.Response;

public record LeadDetailResponseV1(
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
    IList<FollowUpResponseV1> FollowUps,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
