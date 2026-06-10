namespace AutoDealerPro.Modules.Leads.Application.V1.Response;

public record LeadListResponseV1(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string Phone,
    string Type,
    string Status,
    string Message,
    string Priority,
    Guid? AssignedToStaffId,
    DateTime CreatedAt
);
