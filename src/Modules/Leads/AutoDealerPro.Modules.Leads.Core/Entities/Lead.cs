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
    public LeadPriority Priority { get; set; }

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
        Guid vehicleId, string type, string message,
        string? tradeInMake = null, string? tradeInModel = null,
        int? tradeInYear = null, int? tradeInMileage = null, LeadPriority leadPriority = LeadPriority.Low)
    {
        ValidateFirstName(firstName);
        ValidateLastName(lastName);
        ValidateEmail(email);
        ValidatePhone(phone);
        ValidateLeadType(type);
        ValidateMessage(message);
        ValidateTradeInYear(tradeInYear);
        ValidateTradeInMileage(tradeInMileage);

        return new Lead
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email.ToLowerInvariant(),
            Phone = phone,
            VehicleId = vehicleId,
            Type = Enum.Parse<LeadType>(type),
            Status = LeadStatus.New,
            Message = message,
            TradeInMake = tradeInMake,
            TradeInModel = tradeInModel,
            TradeInYear = tradeInYear,
            TradeInMileage = tradeInMileage,
            Priority = leadPriority
        };
    }

    public void UpdateLeadPriority(LeadPriority newPriority)
    {
        Priority = newPriority;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AssignToStaff(Guid staffId)
    {
        AssignedToStaffId = staffId;
        Status = LeadStatus.Assigned;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsContacted(string notes)
    {
        ValidateContactNotes(notes);

        Status = LeadStatus.Contacted;
        ContactedAt = DateTime.UtcNow;
        StaffNotes = notes;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddFollowUp(string notes, DateTime? nextFollowUpDate = null)
    {
        FollowUp followUp = FollowUp.Create(notes, nextFollowUpDate);

        FollowUps.Add(followUp);
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsClosed(bool converted)
    {
        Status = converted ? LeadStatus.Converted : LeadStatus.Lost;
        UpdatedAt = DateTime.UtcNow;
    }

    #region ::: validations :::
    private static void ValidateContactNotes(string notes)
    {
        if (notes.Length > 500)
            throw new ArgumentException("Follow-up notes cannot exceed 500 characters", nameof(notes));
    }

    private static void ValidateFirstName(string firstName)
    {
        if (firstName.Length > 100)
            throw new ArgumentException("First name cannot exceed 100 characters", nameof(firstName));
    }

    private static void ValidateLastName(string lastName)
    {
        if (lastName.Length > 100)
            throw new ArgumentException("Last name cannot exceed 100 characters", nameof(lastName));
    }

    private static void ValidateEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            if (addr.Address != email)
                throw new ArgumentException("Email format is invalid", nameof(email));
        }
        catch
        {
            throw new ArgumentException("Email format is invalid", nameof(email));
        }
    }

    private static void ValidatePhone(string phone)
    {
        if (!System.Text.RegularExpressions.Regex.IsMatch(phone, @"^\+?[\d\s\-()]{10,}$"))
            throw new ArgumentException("Phone format is invalid", nameof(phone));
    }

    private static void ValidateLeadType(string type)
    {
        if (!Enum.TryParse<LeadType>(type, true, out _))
            throw new ArgumentException("Invalid lead type. Valid types: GeneralInquiry, TestDrive, TradeIn", nameof(type));
    }

    private static void ValidateMessage(string message)
    {
        if (message.Length > 1000)
            throw new ArgumentException("Message cannot exceed 1000 characters", nameof(message));
    }

    private static void ValidateTradeInYear(int? tradeInYear)
    {
        if (tradeInYear.HasValue && (tradeInYear < 1900 || tradeInYear > DateTime.UtcNow.Year))
            throw new ArgumentException($"Trade-in year must be between 1900 and {DateTime.UtcNow.Year}", nameof(tradeInYear));
    }

    private static void ValidateTradeInMileage(int? tradeInMileage)
    {
        if (tradeInMileage.HasValue && tradeInMileage < 0)
            throw new ArgumentException("Trade-in mileage cannot be negative", nameof(tradeInMileage));
    }


    #endregion
}