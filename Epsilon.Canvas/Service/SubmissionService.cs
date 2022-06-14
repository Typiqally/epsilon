using System.Text;
using Epsilon.Canvas.Abstractions.Data;
using Epsilon.Canvas.Abstractions.Services;
using Epsilon.Http.Abstractions;
using Epsilon.Http.Abstractions.Json;

namespace Epsilon.Canvas.Service;

public class SubmissionService : HttpService, ISubmissionService
{
    public SubmissionService(HttpClient client) : base(client)
    {
    }

    public async Task<IEnumerable<Submission>> GetAllFromStudent(int courseId, IEnumerable<string> include, int limit = 100)
    {
        var url = new StringBuilder($"v1/courses/{courseId}/students/submissions?per_page={limit}");

        foreach (var parameter in include)
        {
            url.Append($"&include[]={parameter}");
        }

        var uri = new Uri(url.ToString(), UriKind.Relative);
        var request = new HttpRequestMessage(HttpMethod.Get, uri);
        var (_, value) = await Client.SendAsync<IEnumerable<Submission>>(request);

        return value;
    }
}