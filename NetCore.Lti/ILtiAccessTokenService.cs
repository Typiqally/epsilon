using IdentityModel.Client;
using Microsoft.IdentityModel.Tokens;

namespace NetCore.Lti;

public interface ILtiAccessTokenService
{
    string GetAssertionToken(ToolPlatform platform, string localIssuer, SigningCredentials signingCredentials, DateTime? expires = null);

    Task<TokenResponse?> RequestToken(ToolPlatform platform, string assertionToken, string scope);
}