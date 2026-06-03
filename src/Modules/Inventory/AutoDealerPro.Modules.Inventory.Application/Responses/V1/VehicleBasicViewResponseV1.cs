namespace AutoDealerPro.Modules.Inventory.Application.Responses.V1;

public record VehicleBasicViewResponseV1(
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
