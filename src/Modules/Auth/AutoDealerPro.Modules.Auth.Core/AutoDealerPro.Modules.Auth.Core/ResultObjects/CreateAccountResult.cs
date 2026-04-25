using AutoDealerPro.Modules.Auth.Core.ResultObjects.Enums;

namespace AutoDealerPro.Modules.Auth.Core.ResultObjects;

public record CreateAccountResult(bool Created, AccountCreationValidationStatus AccountCreationStatus);
