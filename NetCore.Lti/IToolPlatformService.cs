using IdentityModel.Jwk;

namespace NetCore.Lti;

public interface IToolPlatformService
{
    Task<ToolPlatform?> GetById(string id);
    Task<JsonWebKeySet?> GetJwks(ToolPlatform tenant);
}