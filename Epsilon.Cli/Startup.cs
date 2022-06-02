using Epsilon.Abstractions.Export;
using Epsilon.Canvas;
using Epsilon.Canvas.Abstractions;
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
    private readonly ICanvasModuleCollectionFetcher _collectionFetcher;
    private readonly ICanvasModuleCollectionExporter _collectionExporter;

    public Startup(
        ILogger<Startup> logger,
        IHostApplicationLifetime lifetime,
        IOptions<CanvasSettings> canvasSettings,
        IOptions<ExportSettings> exportSettings,
        ICanvasModuleCollectionFetcher collectionFetcher,
        ICanvasModuleCollectionExporter collectionExporter)
    {
        _logger = logger;
        _canvasSettings = canvasSettings.Value;
        _exportSettings = exportSettings.Value;
        _lifetime = lifetime;
        _collectionFetcher = collectionFetcher;
        _collectionExporter = collectionExporter;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting Epsilon, targeting course: {CourseId}", _canvasSettings.CourseId);
        _logger.LogInformation("Using export format: {Format}", _exportSettings.Format);

        _lifetime.ApplicationStarted.Register(() => Task.Run(ExecuteAsync, cancellationToken));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task ExecuteAsync()
    {
        var modules = await _collectionFetcher.Fetch(_canvasSettings.CourseId);

        var format = _exportSettings.Format.ToLower();
        _collectionExporter.Export(modules, format);

        _lifetime.StopApplication();
    }
}