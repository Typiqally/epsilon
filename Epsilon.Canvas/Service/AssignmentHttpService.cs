using System.Net.Http.Json;
using Epsilon.Abstractions.Http;
using Epsilon.Canvas.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Service;

namespace Epsilon.Canvas.Service;

public class AssignmentHttpService : HttpService, IAssignmentHttpService
{
    private readonly IPaginatorHttpService _paginator;

    public AssignmentHttpService(HttpClient client, IPaginatorHttpService paginator) : base(client)
    {
        _paginator = paginator;
    }

    public async Task<IEnumerable<Assignment>?> GetAll(int courseId, IEnumerable<string> include)
    {
        var uri = $"v1/courses/{courseId}/assignments";
        var query = $"?include[]={string.Join("&include[]=", include)}";

        var pages = await _paginator.GetAllPages<IEnumerable<Assignment>>(HttpMethod.Get, uri + query);
        return pages.SelectMany(static p => p);
    }

    public async Task<Assignment?> GetById(int courseId, int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"v1/courses/{courseId}/assignments/{id}");
        var response = await Client.SendAsync(request);

        return await response.Content.ReadFromJsonAsync<Assignment>();
    }
}