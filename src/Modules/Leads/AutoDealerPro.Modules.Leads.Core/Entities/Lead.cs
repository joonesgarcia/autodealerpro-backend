using AutoDealerPro.Modules.Leads.Core.Enums;
using AutoDealerPro.Shared.Kernel.Types;

namespace AutoDealerPro.Modules.Leads.Core.Entities;

public class Lead : EntityBase
{
    // Customer Info
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string Phone { get; private set; }

    // Inquiry Details
    public Guid VehicleId { get; private set; }
    public LeadType Type { get; private set; }          // GeneralInquiry, TestDrive, TradeIn
    public LeadStatus Status { get; private set; }
    public string Message { get; private set; }

    // Trade-in Info (optional)
    public string? TradeInMake { get; private set; }
    public string? TradeInModel { get; private set; }
    public int? TradeInYear { get; private set; }
    public int? TradeInMileage { get; private set; }

    // Follow-up
    public Guid? AssignedToStaffId { get; private set; }
    public DateTime? ContactedAt { get; private set; }
    public string? StaffNotes { get; private set; }
    public List<FollowUp> FollowUps { get; private set; } = new();

    private Lead() { }

    public static Lead Create(
        string firstName, string lastName, string email, string phone,
        Guid vehicleId, LeadType type, string message,
        string? tradeInMake = null, string? tradeInModel = null,
        int? tradeInYear = null, int? tradeInMileage = null)
    {
        return new Lead
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email.ToLowerInvariant(),
            Phone = phone,
            VehicleId = vehicleId,
            Type = type,
            Status = LeadStatus.New,
            Message = message,
            TradeInMake = tradeInMake,
            TradeInModel = tradeInModel,
            TradeInYear = tradeInYear,
            TradeInMileage = tradeInMileage
        };
    }

    public void AssignToStaff(Guid staffId)
    {
        AssignedToStaffId = staffId;
        Status = LeadStatus.Assigned;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsContacted(string notes)
    {
        Status = LeadStatus.Contacted;
        ContactedAt = DateTime.UtcNow;
        StaffNotes = notes;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddFollowUp(string notes, DateTime? nextFollowUpDate = null)
    {
        var followUp = new FollowUp
        {
            Id = Guid.NewGuid(),
            LeadId = Id,
            Notes = notes,
            CreatedAt = DateTime.UtcNow,
            NextFollowUpDate = nextFollowUpDate
        };
        FollowUps.Add(followUp);
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsClosed(bool converted)
    {
        Status = converted ? LeadStatus.Converted : LeadStatus.Lost;
        UpdatedAt = DateTime.UtcNow;
    }
}