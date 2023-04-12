using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.IdentityModel.Tokens;

namespace NetCore.Lti;

public class LtiAccessTokenService : ILtiAccessTokenService
{
    private readonly HttpMessageInvoker _client;
    private readonly JwtSecurityTokenHandler _tokenHandler;

    public LtiAccessTokenService(HttpMessageInvoker client, JwtSecurityTokenHandler tokenHandler)
    {
        _client = client;
        _tokenHandler = tokenHandler;
    }

    public string GetAssertionToken(ToolPlatform platform, string localIssuer, SigningCredentials signingCredentials, DateTime? expires = null)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, platform.ClientId),
            }),
            Claims = new Dictionary<string, object>
            {
                { JwtRegisteredClaimNames.Jti, Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)) },
            },
            Expires = expires ?? DateTime.UtcNow.AddMinutes(5),
            Issuer = localIssuer,
            Audience = platform.AccessTokenUrl.ToString(),
            SigningCredentials = signingCredentials,
        };

        var token = _tokenHandler.CreateToken(tokenDescriptor);

        return _tokenHandler.WriteToken(token);
    }

    public async Task<TokenResponse?> RequestToken(ToolPlatform platform, string assertionToken, string scope)
    {
        var request = new ClientCredentialsTokenRequest
        {
            Address = platform.AccessTokenUrl.ToString(),
            GrantType = OidcConstants.GrantTypes.ClientCredentials,
            ClientId = platform.ClientId,
            ClientAssertion = new ClientAssertion
            {
                Type = OidcConstants.ClientAssertionTypes.JwtBearer,
                Value = assertionToken,
            },
            Scope = scope,
        };

        return await _client.RequestTokenAsync(request);
    }
}