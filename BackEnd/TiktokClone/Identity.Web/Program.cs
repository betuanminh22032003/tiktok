using System.Text;
using Identity.Application.Models;
using Identity.Application.Interfaces;
using Identity.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Bind JwtSettings from configuration
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>() ?? new JwtSettings();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Token service
// Register application/infrastructure auth and role services
builder.Services.AddScoped<Identity.Application.Interfaces.IAuthService, Identity.Infrastructure.Services.AuthService>();
builder.Services.AddSingleton<Identity.Application.Interfaces.IRoleService, Identity.Infrastructure.Services.RoleService>();

// Configure JWT authentication - read token from HttpOnly cookie named "access_token"
var keyBytes = Encoding.UTF8.GetBytes(jwtSettings.Key ?? string.Empty);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
            ValidateIssuer = !string.IsNullOrEmpty(jwtSettings.Issuer),
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = !string.IsNullOrEmpty(jwtSettings.Audience),
            ValidAudience = jwtSettings.Audience,
            // Map role claim named "role" into ClaimsPrincipal.IsInRole checks
            RoleClaimType = "role",
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        // Read token from cookie so the client doesn't need to add Authorization header
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Cookies.ContainsKey("access_token"))
                {
                    context.Token = context.Request.Cookies["access_token"];
                }
                return Task.CompletedTask;
            }
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
