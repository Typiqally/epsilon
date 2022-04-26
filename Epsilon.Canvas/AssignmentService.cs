using Epsilon.Canvas.Abstractions;
using Epsilon.Canvas.Abstractions.Data;
using Epsilon.Http.Abstractions;
using Epsilon.Http.Json;

namespace Epsilon.Canvas;

public class AssignmentService : HttpService, IAssignmentService
{
    public AssignmentService(HttpClient client) : base(client)
    {
    }

    public async Task<Assignment?> Find(int courseId, int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"v1/courses/{courseId}/assignments/{id}");
        var (response, value) = await Client.SendAsync<Assignment>(request);

        return value;
    }
}