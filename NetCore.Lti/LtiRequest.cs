using System.IdentityModel.Tokens.Jwt;
using IdentityModel.Jwk;
using NetCore.Lti.Extensions;

namespace NetCore.Lti;

public class LtiRequest : JwtSecurityToken
{
    public LtiRequest(string idToken) : base(idToken)
    {
    }

    public string? Type => Claims.GetValue(LtiClaimType.MessageType);

    public string? Version => Claims.GetValue(LtiClaimType.Version);

    public string? DeploymentId => Claims.GetValue(LtiClaimType.DeploymentId);

    public ResourceLink ResourceLink => Claims.GetValue<ResourceLink>(LtiClaimType.ResourceLink);

    public Uri TargetLinkUri => new(Claims.GetValue(LtiClaimType.TargetLinkUri));

    public LearningInformationServices? Lis => Claims.GetValue<LearningInformationServices>(LtiClaimType.LearningInformationServices);

    public Context? Context => Claims.GetValue<Context>(LtiClaimType.Context);

    public ToolPlatformReference? ToolPlatform => Claims.GetValue<ToolPlatformReference>(LtiClaimType.ToolPlatform);

    public LaunchPresentation? LaunchPresentation => Claims.GetValue<LaunchPresentation>(LtiClaimType.LaunchPresentation);

    public IEnumerable<string>? Roles => Claims.GetValue<IEnumerable<string>>(LtiClaimType.Roles);

    public IDictionary<string, string>? Custom => Claims.GetValue<IDictionary<string, string>?>(LtiClaimType.Custom);

    public JsonWebKey GetSigningKey(JsonWebKeySet jwks) => jwks.Keys.Single(k => k.Kid == Header.Kid.Trim('"'));
}