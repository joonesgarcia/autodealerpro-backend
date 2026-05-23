using AutoDealerPro.Modules.Auth.Core.Result.Enums;

namespace AutoDealerPro.Modules.Auth.Core.Result;

public record LoginResult(LoginStatus Status, string? Token = null);
