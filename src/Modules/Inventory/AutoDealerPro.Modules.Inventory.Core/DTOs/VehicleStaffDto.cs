namespace AutoDealerPro.Modules.Inventory.Core.DTOs;
// For staff only (includes business data)
public record VehicleStaffDto(
    Guid Id,
    string Make,
    string Model,
    int Year,
    string PlateNumber,
    string Trim,
    int Mileage,
    string ExteriorColor,
    string InteriorColor,
    string Transmission,
    string FuelType,
    string BodyType,
    decimal PurchasePrice,
    decimal AskingPrice,
    decimal? SellingPrice,
    string Status,
    string? Notes,
    List<string> PhotoUrls,
    int ViewCount,
    DateTime CreatedAt,
    DateTime? SoldAt
);
