using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Epsilon.Cli;

public class Startup : IHostedService
{
    private readonly ILogger<Startup> _logger;
    private readonly IHostApplicationLifetime _lifetime;
    private readonly CanvasSettings _settings;

    public Startup(
        ILogger<Startup> logger,
        IHostApplicationLifetime lifetime,
        IOptions<CanvasSettings> options)
    {
        _logger = logger;
        _lifetime = lifetime;
        _settings = options.Value;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting Epsilon, targeting course: {courseId}", _settings.CourseId);

        _lifetime.ApplicationStarted.Register(() => Task.Run(ExecuteAsync, cancellationToken));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private Task ExecuteAsync()
    {
        // TODO: Execute tasks

        _lifetime.StopApplication();

        return Task.CompletedTask;
    }
}