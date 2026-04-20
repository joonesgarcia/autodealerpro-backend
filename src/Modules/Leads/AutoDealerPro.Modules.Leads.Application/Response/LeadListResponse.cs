namespace AutoDealerPro.Modules.Leads.Application.Response;

public record LeadListResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string Phone,
    string Type,
    string Status,
    string Message,
    Guid? AssignedToStaffId,
    DateTime CreatedAt
);
