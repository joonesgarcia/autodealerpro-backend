using AutoDealerPro.Modules.Inventory.Application.Requests;
using AutoDealerPro.Modules.Inventory.Application.Response;

namespace AutoDealerPro.Modules.Inventory.Application.Interfaces;

public interface IInventoryService
{
    Task<IEnumerable<VehicleListResponse>> GetAvailableVehiclesAsync(int page = 1, int pageSize = 12);
    Task<VehicleDetailResponse?> GetVehicleByIdAsync(Guid id);
    Task<IEnumerable<VehicleListResponse>> SearchVehiclesAsync(VehicleSearchFilterRequest filter);
    Task<VehicleStaffResponse> CreateVehicleAsync(CreateVehicleRequest request);
    Task UpdatePriceAsync(Guid id, UpdatePriceRequest request);
    Task UpdateMileageAsync(Guid id, UpdateMileageRequest request);
    Task AddPhotoAsync(Guid id, AddPhotoRequest request);
    Task MarkAsSoldAsync(Guid id, MarkAsSoldRequest request);
}
