using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using NetCore.Lti.Extensions;
using Microsoft.IdentityModel.Tokens;

namespace NetCore.Lti;

public class LtiTokenValidator : ILtiTokenValidator
{
    private readonly IToolPlatformService _platformService;
    private readonly JwtSecurityTokenHandler _jwtHandler;

    public LtiTokenValidator(IToolPlatformService platformService, JwtSecurityTokenHandler jwtHandler)
    {
        _platformService = platformService;
        _jwtHandler = jwtHandler;
    }

    public LtiTokenValidator(IToolPlatformService platformService) : this(platformService, new JwtSecurityTokenHandler())
    {
    }

    public async Task<Tuple<ClaimsPrincipal, SecurityToken>?> ValidateSignature(ToolPlatform toolPlatform, LtiRequest request, TokenValidationParameters? validationParameters = null)
    {
        var platformJwks = await _platformService.GetJwks(toolPlatform);
        var signingKey = request.GetSigningKey(platformJwks);

        validationParameters ??= new TokenValidationParameters();

        validationParameters.ValidAudience = toolPlatform.ClientId;
        validationParameters.ValidIssuer = toolPlatform.Issuer;
        validationParameters.IssuerSigningKey = signingKey.ToRsaSecurityKey();

        var claimsPrincipal = _jwtHandler.ValidateToken(request.RawData, validationParameters, out var validatedToken);
        return new Tuple<ClaimsPrincipal, SecurityToken>(claimsPrincipal, validatedToken);
    }
}