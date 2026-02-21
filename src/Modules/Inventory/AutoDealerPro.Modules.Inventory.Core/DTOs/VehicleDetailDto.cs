namespace AutoDealerPro.Modules.Inventory.Core.DTOs;
// For detailed view (customers + staff)
public record VehicleDetailDto(
    Guid Id,
    string Make,
    string Model,
    int Year,
    string PlateNumberVIN,
    string Trim,
    int Mileage,
    string ExteriorColor,
    string InteriorColor,
    string Transmission,
    string FuelType,
    string BodyType,
    decimal AskingPrice,
    string Status,
    List<string> PhotoUrls,
    int ViewCount,
    DateTime CreatedAt
);
