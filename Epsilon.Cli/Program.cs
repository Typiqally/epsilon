﻿using Epsilon.Abstractions.Format;
using Epsilon.Canvas;
using Epsilon.Cli;
using Epsilon.Format;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration(static config =>
        {
            config.SetBasePath(Directory.GetCurrentDirectory());
            config.AddJsonFile("appsettings.json");
        })
        .ConfigureServices(static (context, services) =>
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(context.Configuration)
                .CreateLogger();

            services.AddCanvas(context.Configuration.GetSection("Canvas"));
            services.AddScoped<ICsvFormat, CsvFormat>();
            services.AddHostedService<Startup>();
        });
}
await CreateHostBuilder(args)
    .UseSerilog()
    .Build()
    .RunAsync();