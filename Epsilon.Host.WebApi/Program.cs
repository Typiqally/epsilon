using DocumentFormat.OpenXml;
using Epsilon.Abstractions.Component;
using Epsilon.Canvas;
using Epsilon.Component;

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
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCanvas(canvasConfiguration);

builder.Services.AddScoped<IWordDocumentBuilder, WordDocumentBuilder>();

builder.Services.AddComponentFetcher<CompetenceProfile, CompetenceProfileComponentFetcher>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
}

app.UseSwagger();


app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();