using IdentityModel.Jwk;

namespace NetCore.Lti;

public class LtiOptions
{
    public string RedirectUri { get; set; }
    
    public JsonWebKey Jwk { get; set; }
}