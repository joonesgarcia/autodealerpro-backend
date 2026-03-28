namespace AutoDealerPro.Modules.Leads.Application.Exceptions;

public class LeadNotFoundException : Exception
{
    public LeadNotFoundException(Guid leadId)
        : base($"Lead with ID '{leadId}' was not found") { }
}
