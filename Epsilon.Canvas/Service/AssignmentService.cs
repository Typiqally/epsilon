using Epsilon.Canvas.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Services;
using Epsilon.Http.Abstractions;
using Epsilon.Http.Abstractions.Json;
using Microsoft.Extensions.Logging;

namespace Epsilon.Canvas.Service;

public class AssignmentService : HttpService, IAssignmentService
{
    public AssignmentService(HttpClient client) : base(client)
    {
    }

    public async Task<Assignment?> Find(int courseId, int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"v1/courses/{courseId}/assignments/{id}");
        var (_, value) = await Client.SendAsync<Assignment>(request);

        return value;
    }
}