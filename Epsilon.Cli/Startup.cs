using System.ComponentModel.DataAnnotations;
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
        try
        {
            var results = Validate(_canvasSettings).ToArray();
            if (results.Any())
            {
                foreach (var validationResult in results)
                {
                    _logger.LogError("Error: {Message}", validationResult.ErrorMessage);
                }

                _lifetime.StopApplication();
                return;
            }

            _logger.LogInformation("Targeting Canvas course: {CourseId}, at {Url}", _canvasSettings.CourseId, _canvasSettings.ApiUrl);
            var modules = await _collectionFetcher.GetAll(_canvasSettings.CourseId);

            var formats = _exportOptions.Formats.Split(",");
            var exporters = _exporterCollection.DetermineExporters(formats).ToArray();

            _logger.LogInformation("Attempting to use following formats: {Formats}", string.Join(", ", formats));

            foreach (var (format, exporter) in exporters)
            {
                _logger.LogInformation("Exporting to {Format} using {Exporter}...", format, exporter.GetType().Name);
                exporter.Export(modules, format);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occured:");
        }
        finally
        {
            _lifetime.StopApplication();
        }
    }

    private static IEnumerable<ValidationResult> Validate(object model)
    {
        var results = new List<ValidationResult>();
        var context = new ValidationContext(model);

        var isValid = Validator.TryValidateObject(model, context, results, true);

        return results;
    }
}