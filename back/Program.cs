using back.Data;
using back.Security;
using back.Security.Jwt;
using back.Service.Implementation;
using back.Service.Interface;
using back.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Prometheus;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    );
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy
            .WithOrigins("http://localhost:4200", "https://front-weld-pi.vercel.app")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.Services.AddScoped<HashPassword>();
builder.Services.AddScoped<IJwtToken, JwtTokenService>();

builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!)
            )
    };
});

builder.Services.AddScoped<IAuth, AuthImpl>();
builder.Services.AddScoped<ITailingDeposit, TailingDepositImpl>();
builder.Services.AddScoped<ITopographicLandmark, TopographicLandmarkImpl>();
builder.Services.AddScoped<IPiezometer, PiezometerImpl>();
builder.Services.AddScoped<ICoordinateConverter, CoordinateConverterImpl>();
builder.Services.AddScoped<IMap, KeyImpl>();

builder.Services.AddMemoryCache();
builder.Services.AddScoped<CacheService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope()) 
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (dbContext.Database.IsRelational())
    {
        dbContext.Database.Migrate();
    }
}

    app.UseMetricServer();
app.UseHttpMetrics();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowAll");
//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();