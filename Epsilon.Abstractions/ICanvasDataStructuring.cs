using Epsilon.Canvas.Abstractions.Data;

namespace Epsilon.Abstractions;

public interface ICanvasDataStructuring
{
    public Task<IEnumerable<Module>> GatherData(int courseId);
    public void Export(IEnumerable<Module> modules, string format);
}