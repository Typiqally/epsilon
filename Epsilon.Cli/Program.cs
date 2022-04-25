using Epsilon.Cli;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices(static (_, services) => { services.AddHostedService<Startup>(); });

await CreateHostBuilder(args)
    .Build()
    .RunAsync();