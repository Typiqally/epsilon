using Epsilon.Abstractions.Export;
using Epsilon.Canvas;
using Epsilon.Canvas.Abstractions;
using Epsilon.Export;
using Epsilon.Export.Exceptions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Epsilon.Cli;

public class Startup : IHostedService
{
    private readonly ILogger<Startup> _logger;
    private readonly IHostApplicationLifetime _lifetime;
    private readonly ExportOptions _exportOptions;
    private readonly CanvasSettings _canvasSettings;
    private readonly ICanvasModuleCollectionFetcher _collectionFetcher;
    private readonly IModuleExporterCollection _exporterCollection;

    public Startup(
        ILogger<Startup> logger,
        IHostApplicationLifetime lifetime,
        IOptions<CanvasSettings> canvasSettings,
        IOptions<ExportOptions> exportSettings,
        ICanvasModuleCollectionFetcher collectionFetcher,
        IModuleExporterCollection exporterCollection)
    {
        _logger = logger;
        _canvasSettings = canvasSettings.Value;
        _exportOptions = exportSettings.Value;
        _lifetime = lifetime;
        _collectionFetcher = collectionFetcher;
        _exporterCollection = exporterCollection;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _lifetime.ApplicationStarted.Register(() => Task.Run(ExecuteAsync, cancellationToken));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task ExecuteAsync()
    {
        _logger.LogInformation("Targeting Canvas course: {CourseId}, at {Url}", _canvasSettings.CourseId, _canvasSettings.ApiUrl);
        _logger.LogInformation("Using export formats: {Formats}", string.Join(",", _exportOptions.Formats));

        try
        {
            var exporters = _exporterCollection.DetermineExporters(_exportOptions.Formats).ToArray();
            var modules = (await _collectionFetcher.Fetch(_canvasSettings.CourseId)).ToArray();

            foreach (var (format, exporter) in exporters)
            {
                _logger.LogInformation("Exporting to {Format} using {Exporter}...", format, exporter.GetType().Name);
                exporter.Export(modules, format);
            }
        }
        catch (NoExportersFoundException e)
        {
            _logger.LogCritical("An error occured: {Message}", e.Message);
        }
        finally
        {
            _lifetime.StopApplication();
        }
    }
}