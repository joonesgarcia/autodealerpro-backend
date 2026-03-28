namespace AutoDealerPro.Modules.Leads.Application.DTOs;

public record LeadListDto(
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
