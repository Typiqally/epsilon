using IdentityModel.Jwk;
using NetCore.Lti.Data;

namespace NetCore.Lti;

public class ToolPlatformService : IToolPlatformService
{
    private readonly HttpClient _http;
    private readonly IToolPlatformRepository _tenantRepository;

    public ToolPlatformService(HttpClient http, IToolPlatformRepository tenantRepository)
    {
        _http = http;
        _tenantRepository = tenantRepository;
    }

    public async Task<ToolPlatform?> GetById(string id)
    {
        return  await _tenantRepository.SingleOrDefaultAsync(platform => platform.Id == id);
    }

    public async Task<JsonWebKeySet?> GetJwks(ToolPlatform tenant)
    {
        var response = await _http.GetAsync(tenant.JwkSetUrl);
        return new JsonWebKeySet(await response.Content.ReadAsStringAsync());
    }
}