using System.Text;
using Epsilon.Abstractions.Http;
using Epsilon.Abstractions.Http.Json;
using Epsilon.Canvas.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Service;

namespace Epsilon.Canvas.Service;

public class ModuleHttpService : HttpService, IModuleHttpService
{
    public ModuleHttpService(HttpClient client) : base(client)
    {
    }

    public async Task<IEnumerable<Module>?> GetAll(int courseId, IEnumerable<string> include)
    {
        var url = new StringBuilder($"v1/courses/{courseId}/modules");
        var query = $"?include[]={string.Join("&include[]=", include)}";
        
        var request = new HttpRequestMessage(HttpMethod.Get, url + query);
        var (_, value) = await Client.SendAsync<IEnumerable<Module>>(request);

        return value;
    }

    public async Task<Module?> GetById(int courseId, int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"v1/courses/{courseId}/modules/{id}");
        var (_, value) = await Client.SendAsync<Module>(request);

        return value;
    }

    public async Task<IEnumerable<ModuleItem>?> GetAllItems(int courseId, int moduleId, int limit = 100)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"v1/courses/{courseId}/modules/{moduleId}/items?per_page={limit}");
        var (_, value) = await Client.SendAsync<IEnumerable<ModuleItem>>(request);

        return value;
    }
}