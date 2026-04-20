namespace AutoDealerPro.Modules.Leads.Application.Exceptions;

public class DuplicateLeadException(string email)
    : Exception($"A lead with email '{email}' already exists")
{ }
