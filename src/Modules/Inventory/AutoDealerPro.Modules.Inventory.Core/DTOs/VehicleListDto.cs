namespace AutoDealerPro.Modules.Inventory.Core.DTOs;
// For public display (customers)
public record VehicleListDto(
    Guid Id,
    string Make,
    string Model,
    int Year,
    string Trim,
    int Mileage,
    string ExteriorColor,
    string Transmission,
    string FuelType,
    string BodyType,
    decimal AskingPrice,
    string ThumbnailUrl,
    int ViewCount
);
