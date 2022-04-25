using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Epsilon.Cli;

public class Startup : IHostedService
{
    private readonly ILogger<Startup> _logger;
    private readonly IConfiguration _configuration;

    public Startup(ILogger<Startup> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var canvasConfiguration = _configuration.GetRequiredSection("Canvas");
        var canvasCourseId = canvasConfiguration.GetValue<int>("CourseId");
        _logger.LogInformation("Starting Epsilon, targeting course: {courseId}", canvasCourseId);

        return Task.FromResult(0);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(0);
    }
}