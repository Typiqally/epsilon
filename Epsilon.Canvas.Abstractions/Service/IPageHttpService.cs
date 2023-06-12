using Epsilon.Canvas.Abstractions.Model;

namespace Epsilon.Canvas.Abstractions.Service;

public interface IPageHttpService
{
    Task<Page?> UpdateOrCreatePage(int courseId, string pageName, string pageContent);

    Task<Page?> CreatePage(int courseId, string pageName, string pageContent);

    Task<Page?> UpdatePage(int courseId, string pageName, string pageContent);

    Task<string?> GetPageByName(int courseId, string pageName);

    Task<IEnumerable<Page>?> GetAll(int courseId, IEnumerable<string> include);
}