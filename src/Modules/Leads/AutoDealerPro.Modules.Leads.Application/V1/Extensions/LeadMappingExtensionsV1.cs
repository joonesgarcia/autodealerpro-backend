using AutoDealerPro.Modules.Leads.Application.V1.Response;
using AutoDealerPro.Modules.Leads.Core.Entities;

namespace AutoDealerPro.Modules.Leads.Application.V1.Extensions;

public static class LeadMappingExtensions
{
    public static LeadDetailResponseV1 ToDetailResponse(this Lead lead)
    {
        List<FollowUpResponseV1> followUpResponses = [.. lead.FollowUps.Select(f => new FollowUpResponseV1(
                f.Id,
                f.Notes,
                f.CreatedAt,
                f.NextFollowUpDate
            ))];

        return new LeadDetailResponseV1(
            lead.Id,
            lead.FirstName,
            lead.LastName,
            lead.Email,
            lead.Phone,
            lead.VehicleId,
            lead.Type.ToString(),
            lead.Status.ToString(),
            lead.Message,
            lead.TradeInMake,
            lead.TradeInModel,
            lead.TradeInYear,
            lead.TradeInMileage,
            lead.AssignedToStaffId,
            lead.ContactedAt,
            lead.StaffNotes,
            followUpResponses,
            lead.CreatedAt,
            lead.UpdatedAt ?? lead.CreatedAt
        );
    }

    public static LeadListResponseV1 ToListResponse(this Lead lead)
    {
        return new LeadListResponseV1(
            lead.Id,
            lead.FirstName,
            lead.LastName,
            lead.Email,
            lead.Phone,
            lead.Type.ToString(),
            lead.Status.ToString(),
            lead.Message,
            lead.Priority.ToString(),
            lead.AssignedToStaffId,
            lead.CreatedAt
        );
    }
}
