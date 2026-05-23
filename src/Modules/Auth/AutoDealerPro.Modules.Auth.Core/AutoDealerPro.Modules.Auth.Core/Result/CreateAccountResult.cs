using AutoDealerPro.Modules.Auth.Core.Result.Enums;

namespace AutoDealerPro.Modules.Auth.Core.Result;

public record CreateAccountResult(bool Created, AccountCreationValidationStatus AccountCreationStatus);
