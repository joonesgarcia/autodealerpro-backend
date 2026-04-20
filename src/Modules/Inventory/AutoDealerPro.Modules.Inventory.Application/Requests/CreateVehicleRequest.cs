namespace AutoDealerPro.Modules.Inventory.Application.Requests;

public record CreateVehicleRequest(
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
