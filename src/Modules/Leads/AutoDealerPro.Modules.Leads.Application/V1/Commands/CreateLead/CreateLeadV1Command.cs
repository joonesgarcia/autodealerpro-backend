using AutoDealerPro.Modules.Leads.Application.V1.Response;
using MediatR;

namespace AutoDealerPro.Modules.Leads.Application.V1.Commands.CreateLead;

public record CreateLeadV1Command(
    string FirstName,
    string LastName,
    string Email,
    string Phone,
    Guid VehicleId,
    string Type,
    string Message,
    string? TradeInMake = null,
    string? TradeInModel = null,
    int? TradeInYear = null,
    int? TradeInMileage = null
) : IRequest<LeadDetailResponseV1>;
