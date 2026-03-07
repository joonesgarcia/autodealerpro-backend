namespace AutoDealerPro.Modules.Leads.Core.Enums
{
    public enum LeadStatus
    {
        New,               // Just submitted
        Assigned,          // Assigned to staff
        Contacted,         // Staff reached out
        Qualified,         // Serious buyer
        Converted,         // Bought the car
        Lost               // Didn't buy
    }
}
