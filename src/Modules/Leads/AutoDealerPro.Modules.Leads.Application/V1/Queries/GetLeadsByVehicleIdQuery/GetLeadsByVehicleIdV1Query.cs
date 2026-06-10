using AutoDealerPro.Modules.Leads.Application.V1.Response;
using MediatR;

namespace AutoDealerPro.Modules.Leads.Application.V1.Queries.GetLeadsByVehicleIdQuery;

public record GetLeadsByVehicleIdV1Query(Guid VehicleId) : IRequest<IEnumerable<LeadListResponseV1>>;
