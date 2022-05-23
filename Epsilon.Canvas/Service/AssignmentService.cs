using Epsilon.Abstractions.Http;
using Epsilon.Canvas.Abstractions.Data;
using Epsilon.Canvas.Abstractions.Services;
using Epsilon.Http.Json;
using Microsoft.Extensions.Logging;

namespace Epsilon.Canvas.Service;

public class AssignmentService : HttpService, IAssignmentService
{
    private readonly ILogger<AssignmentService> _logger;

    public AssignmentService(HttpClient client, ILogger<AssignmentService> logger) : base(client)
    {
        _logger = logger;
    }

    public async Task<Assignment?> Find(int courseId, int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"v1/courses/{courseId}/assignments/{id}");
        var (response, value) = await Client.SendAsync<Assignment>(request);

        _logger.LogDebug("Fetching assignment #{AssignmentId} from course #{CourseId}", id, courseId);

        return value;
    }
}