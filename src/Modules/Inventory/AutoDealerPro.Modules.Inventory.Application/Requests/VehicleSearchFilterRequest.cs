namespace AutoDealerPro.Modules.Inventory.Application.Requests;

public class VehicleSearchFilterRequest
{
    public string? Make { get; set; }
    public string? Model { get; set; }
    public int? MinYear { get; set; }
    public int? MaxYear { get; set; }
    public decimal? MaxPrice { get; set; }
    public int? MaxMileage { get; set; }
    public string? BodyType { get; set; }
    public string? FuelType { get; set; }
}
