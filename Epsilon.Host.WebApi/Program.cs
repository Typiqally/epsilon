using Epsilon.Abstractions.Component;
using Epsilon.Canvas;
using Epsilon.Canvas.Abstractions.Service;
using Epsilon.Canvas.Service;
using Epsilon.Component.Converters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var canvasConfiguration = builder.Configuration.GetSection("Canvas");

builder.Services.AddCanvas(canvasConfiguration);
builder.Services.AddScoped<ICompetenceProfileConverter, CompetenceProfileConverter>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.UseCors(x => x
    //     .AllowAnyMethod()
    //     .AllowAnyHeader()
    //     .SetIsOriginAllowed(origin => true) 
    //     .AllowCredentials());
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();