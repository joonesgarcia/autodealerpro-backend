namespace AutoDealerPro.Modules.Inventory.Application.Requests.V1.AddVehicle;

public record AddVehicleRequestV1(
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
