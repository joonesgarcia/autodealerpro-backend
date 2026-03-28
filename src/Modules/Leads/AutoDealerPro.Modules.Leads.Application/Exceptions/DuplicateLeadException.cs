namespace AutoDealerPro.Modules.Leads.Application.Exceptions;

public class DuplicateLeadException : Exception
{
    public DuplicateLeadException(string email)
        : base($"A lead with email '{email}' already exists") { }
}
