namespace AutoDealerPro.Modules.Inventory.Application.Responses;

public record VehicleDetailedViewResponse(
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
    decimal AskingPrice,
    string Status,
    List<string> PhotoUrls,
    int ViewCount,
    DateTime CreatedAt
);
