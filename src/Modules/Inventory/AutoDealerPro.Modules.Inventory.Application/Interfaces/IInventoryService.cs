using AutoDealerPro.Modules.Inventory.Application.Requests.V1;
using AutoDealerPro.Modules.Inventory.Application.Requests.V1.AddPhoto;
using AutoDealerPro.Modules.Inventory.Application.Requests.V1.AddVehicle;
using AutoDealerPro.Modules.Inventory.Application.Requests.V1.MarkAsSold;
using AutoDealerPro.Modules.Inventory.Application.Requests.V1.UpdateMileage;
using AutoDealerPro.Modules.Inventory.Application.Requests.V1.UpdatePrice;
using AutoDealerPro.Modules.Inventory.Application.Responses.V1;

namespace AutoDealerPro.Modules.Inventory.Application.Interfaces;

public interface IInventoryService
{
    Task<IEnumerable<VehicleBasicViewResponseV1>> GetAvailableVehiclesAsync(int page = 1, int pageSize = 12);
    Task<IEnumerable<VehicleBasicViewResponseV1>> SearchVehiclesAsync(VehicleFilterRequestV1 filter);

    Task<VehicleDetailedViewResponseV1?> GetVehicleByIdAsync(Guid id);

    Task<VehicleStaffViewResponseV1> CreateVehicleAsync(AddVehicleRequestV1 request);

    Task UpdatePriceAsync(Guid id, UpdatePriceRequestV1 request);
    Task UpdateMileageAsync(Guid id, UpdateMileageRequestV1 request);
    Task AddPhotoAsync(Guid id, AddPhotoRequestV1 request);
    Task MarkAsSoldAsync(Guid id, MarkAsSoldRequestV1 request);
}
