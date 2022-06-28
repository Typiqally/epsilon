using Epsilon.Cli;
using Epsilon.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration(config =>
        {
            config.SetBasePath(Directory.GetCurrentDirectory());
            config.AddJsonFile("appsettings.json");
            config.AddCommandLine(args);
        })
        .ConfigureServices(static (context, services) =>
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(context.Configuration)
                .CreateLogger();

            services.AddCore(context.Configuration);
            services.AddHostedService<Startup>();
        });
}

try
{
    Log.Information("Starting up");
    
    await CreateHostBuilder(args)
        .UseSerilog()
        .Build()
        .RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application was unable to start due to fatal error");
}
finally
{
    Log.CloseAndFlush();
}