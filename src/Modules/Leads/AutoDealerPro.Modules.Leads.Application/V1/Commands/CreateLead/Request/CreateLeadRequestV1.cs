namespace AutoDealerPro.Modules.Leads.Application.V1.Commands.CreateLead.Request;

public record CreateLeadRequestV1(
    string FirstName,
    string LastName,
    string Email,
    string Phone,
    Guid VehicleId,
    string Type,
    string Message,
    string? TradeInMake = null,
    string? TradeInModel = null,
    int? TradeInYear = null,
    int? TradeInMileage = null
);
