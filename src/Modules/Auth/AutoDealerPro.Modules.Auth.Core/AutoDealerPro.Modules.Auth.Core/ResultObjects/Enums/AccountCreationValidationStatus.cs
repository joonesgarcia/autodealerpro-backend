namespace AutoDealerPro.Modules.Auth.Core.ResultObjects;
public enum AccountCreationValidationStatus
{ 
    Valid,
    InvalidUsername,
    InvalidEmail,
    WeakPassword
}