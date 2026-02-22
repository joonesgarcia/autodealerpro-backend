using AutoDealerPro.Modules.Inventory.Core.DTOs;
using AutoDealerPro.Modules.Inventory.Core.Entities;

namespace AutoDealerPro.Modules.Inventory.Core.Repositories;

public interface IVehicleRepository
{
    Task<Vehicle?> GetByIdAsync(Guid id);
    Task<Vehicle?> GetByPlateAsync(string plate);
    Task<IEnumerable<Vehicle>> GetAvailableAsync(int page, int pageSize);
    Task<IEnumerable<Vehicle>> SearchAsync(VehicleSearchFilterDto filter);
    Task<Vehicle> AddAsync(Vehicle vehicle);
    Task UpdateAsync(Vehicle vehicle);
    Task<int> GetAvailableCountAsync();
}
