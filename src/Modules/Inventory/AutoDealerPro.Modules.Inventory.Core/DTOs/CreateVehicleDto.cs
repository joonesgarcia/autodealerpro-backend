namespace AutoDealerPro.Modules.Inventory.Core.DTOs;

// For creating a new vehicle (staff only)
public record CreateVehicleDto(
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
    string? Notes
);
