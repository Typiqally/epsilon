using NetCore.Lti.Data;

namespace NetCore.Lti;

public record ToolPlatform(
    string Id,
    string Name,
    string Issuer,
    string ClientId,
    Uri AccessTokenUrl,
    Uri AuthorizeUrl,
    Uri RedirectUri,
    Uri JwkSetUrl,
    string KeyId
) : Entity<string>(Id);