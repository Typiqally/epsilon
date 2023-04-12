using Epsilon.Canvas.Abstractions.Service;
using Epsilon.Canvas.Service;
using Epsilon.Host.WebApi;
using IdentityModel.Client;
using IdentityModel.Jwk;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NetCore.Lti;
using NetCore.Lti.EntityFrameworkCore;

const string canvas = "canvas-docker";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddSessionStateTempDataProvider();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(static options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(5);

    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.None;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(builder.Configuration["Lti:TargetUri"])
            .AllowCredentials();
    });
});

// Register LTI DbContext
builder.Services.AddDbContext<LtiDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Default");

    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    );
});


// Register LTI and EntityFramework repositories
builder.Services.AddLti(options =>
    {
        options.RedirectUri = "/lti/oidc/callback";
        options.Jwk = new JsonWebKey(builder.Configuration["Lti:Jwk"]);
    })
    .AddEntityFrameworkRepositories<LtiDbContext>();

// Add authentication
builder.Services.AddAuthentication(static options => { options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme; })
    .AddCookie(static options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromDays(365);
        options.Cookie.SameSite = SameSiteMode.None;
        options.Events.OnRedirectToLogin = static context =>
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };
    })
    .AddOAuth(canvas, options =>
    {
        options.ClientId = builder.Configuration["Lti:Canvas:ClientId"];
        options.ClientSecret = builder.Configuration["Lti:Canvas:ClientSecret"];
        options.CallbackPath = "/login/canvas";
        options.AuthorizationEndpoint = builder.Configuration["Lti:Canvas:AuthorizationEndpoint"];
        options.TokenEndpoint = builder.Configuration["Lti:Canvas:TokenEndpoint"];
        options.SaveTokens = true;
    });

// Setup token client using OAuthOptions
builder.Services.AddScoped<TokenClient>(static provider =>
{
    var httpClient = provider.GetRequiredService<IHttpClientFactory>().CreateClient();
    var options = provider.GetRequiredService<IOptionsSnapshot<OAuthOptions>>().Get(canvas);

    return new TokenClient(httpClient, new TokenClientOptions
    {
        Address = options.TokenEndpoint,
        ClientId = options.ClientId,
        ClientSecret = options.ClientSecret,
    });
});

// Setup HTTP clients
builder.Services.AddHttpClient(canvas, (_, client) => { client.BaseAddress = new Uri(builder.Configuration["Lti:Canvas:ApiUrl"]); });
builder.Services.AddHttpClient<IGraphQlHttpService, GraphQlHttpService>(canvas)
    .AddAccessTokenManagement(canvas);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();