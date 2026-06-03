using AutoDealerPro.Modules.Inventory.Application.Interfaces;
using AutoDealerPro.Modules.Inventory.Application.Requests.AddPhoto;
using AutoDealerPro.Modules.Inventory.Application.Requests.AddVehicle;
using AutoDealerPro.Modules.Inventory.Application.Requests.Filter;
using AutoDealerPro.Modules.Inventory.Application.Requests.MarkAsSold;
using AutoDealerPro.Modules.Inventory.Application.Requests.UpdateMileage;
using AutoDealerPro.Modules.Inventory.Application.Requests.UpdatePrice;
using AutoDealerPro.Modules.Inventory.Application.Responses;
using AutoDealerPro.Modules.Inventory.Core.Entities;
using AutoDealerPro.Modules.Inventory.Core.Events;
using AutoDealerPro.Modules.Inventory.Core.Repositories;
using AutoDealerPro.Shared.Abstractions.Events;

namespace AutoDealerPro.Modules.Inventory.Application.Services;

public class InventoryService(IVehicleRepository repository, IEventDispatcher eventDispatcher) : IInventoryService
{
    private readonly IVehicleRepository _repository = repository;
    private readonly IEventDispatcher _eventDispatcher = eventDispatcher;

    public async Task<IEnumerable<VehicleBasicViewResponse>> GetAvailableVehiclesAsync(int page = 1, int pageSize = 12)
    {
        IEnumerable<Core.Entities.Vehicle> vehicles = await _repository.GetAvailableAsync(page, pageSize);
        return vehicles.Select(v => new VehicleBasicViewResponse(
            v.Id, v.Make, v.Model, v.Year, v.Trim, v.Mileage, v.ExteriorColor, v.Transmission, v.FuelType, v.BodyType, v.AskingPrice, v.PhotoUrls.FirstOrDefault() ?? "", v.ViewCount
        ));
    }

    public async Task<VehicleDetailedViewResponse?> GetVehicleByIdAsync(Guid id)
    {
        Core.Entities.Vehicle? vehicle = await _repository.GetByIdAsync(id);
        if (vehicle == null) return null;
        vehicle.IncrementViewCount();
        await _repository.UpdateAsync(vehicle);
        return new VehicleDetailedViewResponse(
            vehicle.Id, vehicle.Make, vehicle.Model, vehicle.Year, vehicle.PlateNumber, vehicle.Trim, vehicle.Mileage, vehicle.ExteriorColor, vehicle.InteriorColor, vehicle.Transmission, vehicle.FuelType, vehicle.BodyType, vehicle.AskingPrice, vehicle.Status.ToString(), vehicle.PhotoUrls, vehicle.ViewCount, vehicle.CreatedAt
        );
    }

    public async Task<IEnumerable<VehicleBasicViewResponse>> SearchVehiclesAsync(VehicleFilterRequest filter)
    {
        IEnumerable<Core.Entities.Vehicle> query = await _repository.GetAvailableAsync(1, 1000);

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

        List<Core.Entities.Vehicle> vehicles = [.. query];
        return vehicles.Select(v => new VehicleBasicViewResponse(
            v.Id, v.Make, v.Model, v.Year, v.Trim, v.Mileage, v.ExteriorColor, v.Transmission, v.FuelType, v.BodyType, v.AskingPrice, v.PhotoUrls.FirstOrDefault() ?? "", v.ViewCount
        ));
    }

    public async Task<VehicleStaffViewResponse> CreateVehicleAsync(AddVehicleRequest request)
    {
        Vehicle vehicle = Vehicle.Create(
            request.Make, request.Model, request.Year, request.PlateNumber, request.Trim, request.Mileage, request.ExteriorColor, request.InteriorColor, request.Transmission, request.FuelType, request.BodyType, request.PurchasePrice, request.AskingPrice, request.Notes
        );
        await _repository.AddAsync(vehicle);
        return new VehicleStaffViewResponse(
            vehicle.Id, vehicle.Make, vehicle.Model, vehicle.Year, vehicle.PlateNumber, vehicle.Trim, vehicle.Mileage, vehicle.ExteriorColor, vehicle.InteriorColor, vehicle.Transmission, vehicle.FuelType, vehicle.BodyType, vehicle.PurchasePrice, vehicle.AskingPrice, vehicle.SellingPrice, vehicle.Status.ToString(), vehicle.Notes, vehicle.PhotoUrls, vehicle.ViewCount, vehicle.CreatedAt, vehicle.SoldAt
        );
    }

    public async Task UpdatePriceAsync(Guid id, UpdatePriceRequest request)
    {
        Core.Entities.Vehicle? vehicle = await _repository.GetByIdAsync(id);
        if (vehicle == null) throw new ArgumentException("Vehicle not found");
        vehicle.UpdatePrice(request.NewPrice);
        await _repository.UpdateAsync(vehicle);
    }

    public async Task UpdateMileageAsync(Guid id, UpdateMileageRequest request)
    {
        Core.Entities.Vehicle? vehicle = await _repository.GetByIdAsync(id);
        if (vehicle == null) throw new ArgumentException("Vehicle not found");
        vehicle.UpdateMileage(request.NewMileage);
        await _repository.UpdateAsync(vehicle);
    }

    public async Task AddPhotoAsync(Guid id, AddPhotoRequest request)
    {
        Core.Entities.Vehicle? vehicle = await _repository.GetByIdAsync(id);
        if (vehicle == null) throw new ArgumentException("Vehicle not found");
        vehicle.AddPhoto(request.PhotoUrl);
        await _repository.UpdateAsync(vehicle);
    }

    public async Task MarkAsSoldAsync(Guid id, MarkAsSoldRequest request)
    {
        Core.Entities.Vehicle? vehicle = await _repository.GetByIdAsync(id);
        if (vehicle == null) throw new ArgumentException("Vehicle not found");

        vehicle.MarkAsSold(request.SellingPrice);
        await _repository.UpdateAsync(vehicle);

        // Publish after persisting — any module that cares about this fact reacts here.
        // In-process now; swap dispatcher for a broker impl when extracting to microservices.
        await _eventDispatcher.Publish(new VehicleSoldEvent(vehicle.Id, request.SellingPrice));
    }
}
