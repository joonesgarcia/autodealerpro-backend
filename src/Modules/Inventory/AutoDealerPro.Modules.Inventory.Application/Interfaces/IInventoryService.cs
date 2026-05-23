using AutoDealerPro.Modules.Inventory.Application.Requests.AddPhoto;
using AutoDealerPro.Modules.Inventory.Application.Requests.AddVehicle;
using AutoDealerPro.Modules.Inventory.Application.Requests.Filter;
using AutoDealerPro.Modules.Inventory.Application.Requests.MarkAsSold;
using AutoDealerPro.Modules.Inventory.Application.Requests.UpdateMileage;
using AutoDealerPro.Modules.Inventory.Application.Requests.UpdatePrice;
using AutoDealerPro.Modules.Inventory.Application.Responses;

namespace AutoDealerPro.Modules.Inventory.Application.Interfaces;

public interface IInventoryService
{
    Task<IEnumerable<VehicleBasicViewResponse>> GetAvailableVehiclesAsync(int page = 1, int pageSize = 12);
    Task<IEnumerable<VehicleBasicViewResponse>> SearchVehiclesAsync(VehicleFilterRequest filter);

    Task<VehicleDetailedViewResponse?> GetVehicleByIdAsync(Guid id);

    Task<VehicleStaffViewResponse> CreateVehicleAsync(AddVehicleRequest request);

    Task UpdatePriceAsync(Guid id, UpdatePriceRequest request);
    Task UpdateMileageAsync(Guid id, UpdateMileageRequest request);
    Task AddPhotoAsync(Guid id, AddPhotoRequest request);
    Task MarkAsSoldAsync(Guid id, MarkAsSoldRequest request);
}
