using AutoDealerPro.Modules.Inventory.Core.Entities;
using AutoDealerPro.Modules.Inventory.Core.Repositories;
using AutoDealerPro.Modules.Inventory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AutoDealerPro.Modules.Inventory.Infrastructure.Repositories;

public class VehicleRepository(InventoryDbContext context) : IVehicleRepository
{
    private readonly InventoryDbContext _context = context;

    public async Task<Vehicle?> GetByIdAsync(Guid id)
        => await _context.Vehicles.FindAsync(id);

    public async Task<Vehicle?> GetByPlateAsync(string plateNumber)
        => await _context.Vehicles
            .FirstOrDefaultAsync(v => v.PlateNumber.ToLower() == plateNumber.ToLower());


    public async Task<IEnumerable<Vehicle>> GetAvailableAsync(int page, int pageSize)
    {
        var skip = (page - 1) * pageSize;
        return await _context.Vehicles
            .Where(v => v.Status == VehicleStatus.Available)
            .OrderByDescending(v => v.CreatedAt)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Vehicle>> SearchAsync(VehicleSearchFilter filter)
    {
        var query = _context.Vehicles.Where(v => v.Status == VehicleStatus.Available);

        if (!string.IsNullOrEmpty(filter.Make))
            query = query.Where(v => v.Make.ToLower() == filter.Make.ToLower());

        if (!string.IsNullOrEmpty(filter.Model))
            query = query.Where(v => v.Model.ToLower().Contains(filter.Model.ToLower()));

        if (filter.MinYear.HasValue)
            query = query.Where(v => v.Year >= filter.MinYear.Value);

        if (filter.MaxYear.HasValue)
            query = query.Where(v => v.Year <= filter.MaxYear.Value);

        if (filter.MaxPrice.HasValue)
            query = query.Where(v => v.AskingPrice <= filter.MaxPrice.Value);

        if (filter.MaxMileage.HasValue)
            query = query.Where(v => v.Mileage <= filter.MaxMileage.Value);

        if (!string.IsNullOrEmpty(filter.BodyType))
            query = query.Where(v => v.BodyType.ToLower() == filter.BodyType.ToLower());

        if (!string.IsNullOrEmpty(filter.FuelType))
            query = query.Where(v => v.FuelType.ToLower() == filter.FuelType.ToLower());

        return await query
            .OrderByDescending(v => v.CreatedAt)
            .Take(50)
            .ToListAsync();
    }
    public async Task<Vehicle> AddAsync(Vehicle vehicle)
    {
        await _context.Vehicles.AddAsync(vehicle);
        await _context.SaveChangesAsync();
        return vehicle;
    }

    public async Task UpdateAsync(Vehicle vehicle)
    {
        _context.Vehicles.Update(vehicle);
        await _context.SaveChangesAsync();
    }

    public async Task<int> GetAvailableCountAsync()
        => await _context.Vehicles.CountAsync(v => v.Status == VehicleStatus.Available);
}
