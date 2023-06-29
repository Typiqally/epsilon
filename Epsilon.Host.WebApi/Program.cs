using Epsilon.Abstractions.Component;
using Epsilon.Abstractions.Service;
using Epsilon.Canvas;
using Epsilon.Component;
using Epsilon.Service;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var canvasConfiguration = builder.Configuration.GetSection("Canvas");

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(builder.Configuration["Lti:TargetUri"])
              .AllowCredentials();
    });
});

builder.Services.AddControllers();

builder.Services.AddRouting(static options => options.LowercaseUrls = true);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCanvas(canvasConfiguration);

builder.Services.AddScoped<IFilterService, FilterService>();
builder.Services.AddScoped<ICompetenceDocumentService, CompetenceDocumentService>();
builder.Services.AddScoped<ICompetenceComponentService, CompetenceComponentService>(static (services) => new CompetenceComponentService(
    new Dictionary<string, ICompetenceComponentFetcher>
    {
        { "persona_page", services.GetRequiredService<ICompetenceComponentFetcher<PersonaPage>>() },
        { "competence_profile", services.GetRequiredService<ICompetenceComponentFetcher<CompetenceProfile>>() },
        { "kpi_matrix", services.GetRequiredService<ICompetenceComponentFetcher<KpiMatrixCollection>>() },
        { "kpi_table", services.GetRequiredService<ICompetenceComponentFetcher<KpiTable>>() },
    }
));

builder.Services.AddScoped<ICompetenceComponentFetcher<PersonaPage>, PersonaPageComponentFetcher>();
builder.Services.AddScoped<ICompetenceComponentFetcher<CompetenceProfile>, CompetenceProfileComponentFetcher>();
builder.Services.AddScoped<ICompetenceComponentFetcher<KpiMatrixCollection>, KpiMatrixComponentFetcher>();
builder.Services.AddScoped<ICompetenceComponentFetcher<KpiTable>, KpiTableComponentFetcher>();

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto, });

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
}

app.UseSwagger(static options =>
{
    options.PreSerializeFilters.Add(static (swagger, request) =>
    {
        if (request.Headers.TryGetValue("X-Forwarded-Proto", out var scheme)
            && request.Headers.TryGetValue("X-Forwarded-Prefix", out var prefix))
        {
            swagger.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{scheme}://{request.Host}/{prefix}", }, };
        }
    });
});

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();