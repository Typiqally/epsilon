using System.Text;
using Epsilon.Canvas.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Service;
using Epsilon.Http.Abstractions;

namespace Epsilon.Canvas.Service;

public class SubmissionService : HttpService, ISubmissionService
{
    private readonly IPaginatorService _paginator;

    public SubmissionService(HttpClient client, IPaginatorService paginator) : base(client)
    {
        _paginator = paginator;
    }

    public async Task<IEnumerable<Submission>> GetAllFromStudent(int courseId, IEnumerable<string> include, int limit = 100)
    {
        var url = new StringBuilder($"v1/courses/{courseId}/students/submissions");
        var query = $"?include[]={string.Join("&include[]=", include)}";

        var responses = await _paginator.FetchAll<IEnumerable<Submission>>(HttpMethod.Get, url + query);
        return responses.SelectMany(static r => r);
    }
}