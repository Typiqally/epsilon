using Epsilon.Canvas.Abstractions;
using Epsilon.Canvas.Abstractions.Data;
using Epsilon.Http.Abstractions;
using Epsilon.Http.Json;

namespace Epsilon.Canvas;

public class ModuleService : HttpService, IModuleService
{
    public ModuleService(HttpClient client) : base(client)
    {
    }

    public async Task<IEnumerable<Module>?> All(int courseId)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"v1/courses/{courseId}/modules");
        var (response, value) = await Client.SendAsync<IEnumerable<Module>>(request);

        return value;
    }

    public async Task<Module?> Find(int courseId, int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"v1/courses/{courseId}/modules/{id}");
        var (response, value) = await Client.SendAsync<Module>(request);

        return value;
    }

    public async Task<IEnumerable<ModuleItem>?> AllItems(int courseId, int moduleId, int count = 1000)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"v1/courses/{courseId}/modules/{moduleId}/items?per_page={count}");
        var (response, value) = await Client.SendAsync<IEnumerable<ModuleItem>>(request);

        return value;
    }
}