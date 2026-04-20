namespace AutoDealerPro.Modules.Leads.Application.Exceptions;

public class LeadNotFoundException(Guid leadId)
    : Exception($"Lead with ID '{leadId}' was not found")
{ }
