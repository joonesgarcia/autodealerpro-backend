namespace AutoDealerPro.Modules.Leads.Application.Requests;

public record CreateLeadRequest(
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
