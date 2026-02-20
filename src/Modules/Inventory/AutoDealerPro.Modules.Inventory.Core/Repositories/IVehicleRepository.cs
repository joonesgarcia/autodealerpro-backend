using AutoDealerPro.Modules.Inventory.Core.Entities;

namespace AutoDealerPro.Modules.Inventory.Core.Repositories;

public interface IVehicleRepository
{
    Task<Vehicle?> GetByIdAsync(Guid id);
    Task<Vehicle?> GetByPlateAsync(string plate);
    Task<IEnumerable<Vehicle>> GetAvailableAsync(int page, int pageSize);
    Task<IEnumerable<Vehicle>> SearchAsync(VehicleSearchFilter filter);
    Task<Vehicle> AddAsync(Vehicle vehicle);
    Task UpdateAsync(Vehicle vehicle);
    Task<int> GetAvailableCountAsync();
}

public class VehicleSearchFilter
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
