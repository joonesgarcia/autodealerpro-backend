namespace AutoDealerPro.Modules.Leads.Core.DTOs;

public record CreateLeadDto(
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