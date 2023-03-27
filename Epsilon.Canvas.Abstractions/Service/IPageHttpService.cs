using Epsilon.Canvas.Abstractions.Model;

namespace Epsilon.Canvas.Abstractions.Service;

public interface IPageHttpService
{
    Task<string> GetPageByName(int courseId, string pageName);

    Task<IEnumerable<Page>?> GetAll(int courseId, IEnumerable<string> include);
}