using NetCore.Lti.Extensions;

namespace NetCore.Lti.DeepLinking;

public static class DeepLinkingLtiRequestExtensions
{
    public static DeepLinkingSettings? DeepLinkingSettings(this LtiRequest request)
    {
        return request.Claims.GetValue<DeepLinkingSettings>(LtiDeepLinkingClaimType.DeepLinkingSettings);
    }
}