using IdentityModel;
using IdentityModel.Client;

namespace NetCore.Lti;

public record LtiOpenIdConnectLaunch(
    string Issuer,
    string LoginHint,
    string ClientId,
    Uri TargetLinkUri,
    string EncodedLtiMessageHint,
    string LtiStorageTarget
)
{
    public string CreateAuthorizeUrl(Uri baseUrl, Uri redirectUri, string nonce, string? state = null)
    {
        var additionalParameters = new Parameters(new[]
        {
            new KeyValuePair<string, string>("lti_message_hint", EncodedLtiMessageHint),
        });

        var requestUrl = new RequestUrl(baseUrl.ToString());
        return requestUrl.CreateAuthorizeUrl(
            clientId: ClientId,
            responseType: OidcConstants.ResponseTypes.IdToken,
            responseMode: OidcConstants.ResponseModes.FormPost,
            scope: OidcConstants.StandardScopes.OpenId,
            redirectUri: redirectUri.ToString(),
            state: state,
            nonce: nonce,
            loginHint: LoginHint,
            prompt: OidcConstants.PromptModes.None,
            extra: additionalParameters
        );
    }
}