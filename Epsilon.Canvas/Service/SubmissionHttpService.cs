using System.Text;
using Epsilon.Abstractions.Http;
using Epsilon.Canvas.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Service;

namespace Epsilon.Canvas.Service;

public class SubmissionHttpService : HttpService, ISubmissionHttpService
{
    private readonly IPaginatorHttpService _paginator;

    public SubmissionHttpService(HttpClient client, IPaginatorHttpService paginator) : base(client)
    {
        _paginator = paginator;
    }

    public async Task<IEnumerable<Submission>> GetAllFromStudent(int courseId, IEnumerable<string> include, int limit = 100)
    {
        var url = new StringBuilder($"v1/courses/{courseId}/students/submissions");
        var query = $"?include[]={string.Join("&include[]=", include)}";

        var responses = await _paginator.GetAllPages<IEnumerable<Submission>>(HttpMethod.Get, url + query);
        return responses.SelectMany(static r => r);
    }
}