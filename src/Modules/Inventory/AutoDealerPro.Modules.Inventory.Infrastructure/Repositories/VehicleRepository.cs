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
