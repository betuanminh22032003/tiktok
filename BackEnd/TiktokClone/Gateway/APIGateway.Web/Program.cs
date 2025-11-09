using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Load ocelot config
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
// Allow environment-specific ocelot config, e.g. ocelot.Development.json
builder.Configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

// JWT setup - read authority from configuration (allows env override)
var identityAuthority = builder.Configuration["Identity:Authority"] ?? Environment.GetEnvironmentVariable("IDENTITY_AUTHORITY") ?? "http://identity-service:5000";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = identityAuthority; // IdentityServer / Keycloak / Auth server
        options.RequireHttpsMetadata = false;
        options.Audience = "api-gateway";
    });

builder.Services.AddOcelot();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

await app.UseOcelot();

app.Run();
