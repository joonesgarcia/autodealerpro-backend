using AutoDealerPro.Modules.Inventory.Application.Interfaces;
using AutoDealerPro.Modules.Inventory.Application.Requests;
using AutoDealerPro.Modules.Inventory.Application.Response;
using AutoDealerPro.Modules.Inventory.Core.Repositories;

namespace AutoDealerPro.Modules.Inventory.Application.Services;

public class InventoryService(IVehicleRepository repository) : IInventoryService
{
    private readonly IVehicleRepository _repository = repository;

    public async Task<IEnumerable<VehicleListResponse>> GetAvailableVehiclesAsync(int page = 1, int pageSize = 12)
    {
        var vehicles = await _repository.GetAvailableAsync(page, pageSize);
        return vehicles.Select(v => new VehicleListResponse(
            v.Id, v.Make, v.Model, v.Year, v.Trim, v.Mileage, v.ExteriorColor, v.Transmission, v.FuelType, v.BodyType, v.AskingPrice, v.PhotoUrls.FirstOrDefault() ?? "", v.ViewCount
        ));
    }

    public async Task<VehicleDetailResponse?> GetVehicleByIdAsync(Guid id)
    {
        var vehicle = await _repository.GetByIdAsync(id);
        if (vehicle == null) return null;
        vehicle.IncrementViewCount();
        await _repository.UpdateAsync(vehicle);
        return new VehicleDetailResponse(
            vehicle.Id, vehicle.Make, vehicle.Model, vehicle.Year, vehicle.PlateNumber, vehicle.Trim, vehicle.Mileage, vehicle.ExteriorColor, vehicle.InteriorColor, vehicle.Transmission, vehicle.FuelType, vehicle.BodyType, vehicle.AskingPrice, vehicle.Status.ToString(), vehicle.PhotoUrls, vehicle.ViewCount, vehicle.CreatedAt
        );
    }

    public async Task<IEnumerable<VehicleListResponse>> SearchVehiclesAsync(VehicleSearchFilterRequest filter)
    {
        var query = _repository
            .GetAvailableAsync(1, 1000) // get all available, filter in memory for now
            .Result
            .AsQueryable();

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

        var vehicles = query.ToList();
        return vehicles.Select(v => new VehicleListResponse(
            v.Id, v.Make, v.Model, v.Year, v.Trim, v.Mileage, v.ExteriorColor, v.Transmission, v.FuelType, v.BodyType, v.AskingPrice, v.PhotoUrls.FirstOrDefault() ?? "", v.ViewCount
        ));
    }

    public async Task<VehicleStaffResponse> CreateVehicleAsync(CreateVehicleRequest request)
    {
        var vehicle = AutoDealerPro.Modules.Inventory.Core.Entities.Vehicle.Create(
            request.Make, request.Model, request.Year, request.PlateNumber, request.Trim, request.Mileage, request.ExteriorColor, request.InteriorColor, request.Transmission, request.FuelType, request.BodyType, request.PurchasePrice, request.AskingPrice, request.Notes
        );
        await _repository.AddAsync(vehicle);
        return new VehicleStaffResponse(
            vehicle.Id, vehicle.Make, vehicle.Model, vehicle.Year, vehicle.PlateNumber, vehicle.Trim, vehicle.Mileage, vehicle.ExteriorColor, vehicle.InteriorColor, vehicle.Transmission, vehicle.FuelType, vehicle.BodyType, vehicle.PurchasePrice, vehicle.AskingPrice, vehicle.SellingPrice, vehicle.Status.ToString(), vehicle.Notes, vehicle.PhotoUrls, vehicle.ViewCount, vehicle.CreatedAt, vehicle.SoldAt
        );
    }

    public async Task UpdatePriceAsync(Guid id, UpdatePriceRequest request)
    {
        var vehicle = await _repository.GetByIdAsync(id);
        if (vehicle == null) throw new ArgumentException("Vehicle not found");
        vehicle.UpdatePrice(request.NewPrice);
        await _repository.UpdateAsync(vehicle);
    }

    public async Task UpdateMileageAsync(Guid id, UpdateMileageRequest request)
    {
        var vehicle = await _repository.GetByIdAsync(id);
        if (vehicle == null) throw new ArgumentException("Vehicle not found");
        vehicle.UpdateMileage(request.NewMileage);
        await _repository.UpdateAsync(vehicle);
    }

    public async Task AddPhotoAsync(Guid id, AddPhotoRequest request)
    {
        var vehicle = await _repository.GetByIdAsync(id);
        if (vehicle == null) throw new ArgumentException("Vehicle not found");
        vehicle.AddPhoto(request.PhotoUrl);
        await _repository.UpdateAsync(vehicle);
    }

    public async Task MarkAsSoldAsync(Guid id, MarkAsSoldRequest request)
    {
        var vehicle = await _repository.GetByIdAsync(id);
        if (vehicle == null) throw new ArgumentException("Vehicle not found");
        vehicle.MarkAsSold(request.SellingPrice);
        await _repository.UpdateAsync(vehicle);
    }
}
