using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace NetCore.Lti;

public interface ILtiTokenValidator
{
     Task<Tuple<ClaimsPrincipal, SecurityToken>?> ValidateSignature(ToolPlatform toolPlatform, LtiRequest request, TokenValidationParameters? validationParameters = null);
}