using Epsilon.Abstractions.Http;
using Epsilon.Canvas.Abstractions.Data;
using Epsilon.Canvas.Abstractions.Services;
using Epsilon.Http.Json;
using Microsoft.Extensions.Logging;

namespace Epsilon.Canvas.Service;

public class ModuleService : HttpService, IModuleService
{
    private readonly ILogger<ModuleService> _logger;

    public ModuleService(HttpClient client, ILogger<ModuleService> logger) : base(client)
    {
        _logger = logger;
    }

    public async Task<IEnumerable<Module>?> All(int courseId)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"v1/courses/{courseId}/modules");
        var (response, value) = await Client.SendAsync<IEnumerable<Module>>(request);

        _logger.LogDebug("Fetching modules from course #{CourseId}", courseId);

        return value;
    }

    public async Task<Module?> Find(int courseId, int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"v1/courses/{courseId}/modules/{id}");
        var (response, value) = await Client.SendAsync<Module>(request);

        _logger.LogDebug("Fetching module #{ModuleId} from course #{CourseId}", id, courseId);

        return value;
    }

    public async Task<IEnumerable<ModuleItem>?> AllItems(int courseId, int moduleId, int count = 1000)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"v1/courses/{courseId}/modules/{moduleId}/items?per_page={count}");
        var (response, value) = await Client.SendAsync<IEnumerable<ModuleItem>>(request);

        _logger.LogDebug("Fetching module #{ModuleId} items from course #{CourseId}", moduleId, courseId);

        return value;
    }
}