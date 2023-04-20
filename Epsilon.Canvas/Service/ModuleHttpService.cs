using System.Net.Http.Json;
using System.Text;
using Epsilon.Abstractions.Http;
using Epsilon.Canvas.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Service;

namespace Epsilon.Canvas.Service;

public class ModuleHttpService : HttpService, IModuleHttpService
{
    public ModuleHttpService(HttpClient client)
        : base(client)
    {
    }

    public async Task<IEnumerable<CourseModule>?> GetAll(int courseId, IEnumerable<string> include)
    {
        var url = new StringBuilder($"v1/courses/{courseId}/modules");
        var query = $"?include[]={string.Join("&include[]=", include)}";

        using var request = new HttpRequestMessage(HttpMethod.Get, url + query);
        var response = await Client.SendAsync(request);

        return await response.Content.ReadFromJsonAsync<IEnumerable<CourseModule>>();
    }

    public async Task<CourseModule?> GetById(int courseId, int id)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"v1/courses/{courseId}/modules/{id}");
        var response = await Client.SendAsync(request);

        return await response.Content.ReadFromJsonAsync<CourseModule>();
    }

    public async Task<IEnumerable<ModuleItem>?> GetAllItems(int courseId, int moduleId, int limit = 100)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"v1/courses/{courseId}/modules/{moduleId}/items?per_page={limit}");
        var response = await Client.SendAsync(request);

        return await response.Content.ReadFromJsonAsync<IEnumerable<ModuleItem>>();
    }
}