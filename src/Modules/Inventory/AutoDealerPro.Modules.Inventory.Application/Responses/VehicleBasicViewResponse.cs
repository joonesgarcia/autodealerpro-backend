namespace AutoDealerPro.Modules.Inventory.Application.Responses;

public record VehicleBasicViewResponse(
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
