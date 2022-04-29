using System.Net.Http.Headers;
using Epsilon.Canvas;
using Epsilon.Canvas.Abstractions;
using Epsilon.Cli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

const string canvasHttpClient = "CanvasHttpClient";

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
            services.AddHttpClient(
                canvasHttpClient, static (provider, client) =>
                {
                    var settings = provider.GetRequiredService<IOptions<CanvasSettings>>().Value;

                    client.BaseAddress = settings.ApiUrl;
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", settings.AccessToken);
                });
            services.AddHttpClient<IModuleService, ModuleService>(canvasHttpClient);
            services.AddHttpClient<IAssignmentService, AssignmentService>(canvasHttpClient);
            services.AddHttpClient<IOutcomeService, OutcomeService>(canvasHttpClient);
            services.AddHostedService<Startup>();
        });

await CreateHostBuilder(args)
    .Build()
    .RunAsync();