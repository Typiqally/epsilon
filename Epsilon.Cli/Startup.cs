using Epsilon.Abstractions;
using Epsilon.Canvas;
using Epsilon.Canvas.Abstractions.Data;
using Epsilon.Export;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Epsilon.Cli;

public class Startup : IHostedService
{
    private readonly ILogger<Startup> _logger;
    private readonly IHostApplicationLifetime _lifetime;
    private readonly ExportSettings _exportSettings;
    private readonly CanvasSettings _canvasSettings;
    private readonly IMainLogic _mainLogic;

    public Startup(
        ILogger<Startup> logger,
        IHostApplicationLifetime lifetime,
        IOptions<CanvasSettings> canvasSettings,
        IOptions<ExportSettings> exportSettings,
        IMainLogic mainLogic)
    {
        _logger = logger;
        _lifetime = lifetime;
        _canvasSettings = canvasSettings.Value;
        _exportSettings = exportSettings.Value;
        _mainLogic = mainLogic;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting Epsilon, targeting course: {CourseId}", _canvasSettings.CourseId);
        _logger.LogInformation("Using export format: {format}", _exportSettings.Format);

        _lifetime.ApplicationStarted.Register(() => Task.Run(ExecuteAsync, cancellationToken));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task ExecuteAsync()
    {
        IEnumerable<Module> modules = await _mainLogic.GatherData(_canvasSettings.CourseId);
        
        Export(modules);

        _lifetime.StopApplication();
    }

    private void Export(IEnumerable<Module> modules)
    {
        var format = _exportSettings.Format.ToLower();
        _mainLogic.Export(modules, format);
    }
}