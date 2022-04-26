using Epsilon.Cli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration(static config =>
        {
            config.SetBasePath(Directory.GetCurrentDirectory());
            config.AddJsonFile("appsettings.json");
        })
        .ConfigureServices(static (context, services) =>
        {
            services.Configure<CanvasSettings>(context.Configuration.GetSection("Canvas"));
            services.AddHostedService<Startup>();
        });

await CreateHostBuilder(args)
    .Build()
    .RunAsync();