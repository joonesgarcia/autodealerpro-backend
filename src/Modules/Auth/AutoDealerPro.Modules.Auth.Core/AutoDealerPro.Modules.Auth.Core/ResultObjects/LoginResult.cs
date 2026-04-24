using AutoDealerPro.Modules.Auth.Core.ResultObjects.Enums;

namespace AutoDealerPro.Modules.Auth.Core.ResultObjects;

public record LoginResult(LoginStatus Status, string? Token = null);
