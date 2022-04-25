using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Epsilon.Cli;

public class Startup : IHostedService
{
    private readonly ILogger<Startup> _logger;
    
    public Startup(ILogger<Startup> logger)
    {
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting Epsilon");

        return Task.FromResult(0);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(0);
    }
}