#region Usings
using Application;
using Application.Contracts;
using Application.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.SeedData;
using Infrastructure.Security;
using Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Web.Extensions;
using Web.Services;
#endregion

var builder = WebApplication.CreateBuilder(args);

#region Just Application Layer Configure Services
builder.Services.AddApplicationServices();
#endregion

#region Just Infrastructure Layer Configure Services
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddSingleton<IConnectionMultiplexer>(opt =>
{
    var options = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis") ?? string.Empty, true);
    return ConnectionMultiplexer.Connect(options);
});

builder.Services.AddIdentityService(builder.Configuration);
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddScoped<IUnitOWork, UnitOWork>();
#endregion

#region General
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocumentation();
#endregion

#region Same Origin Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
        //policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
#endregion

builder.Services.AddSingleton<ICurrentUserService, CurrentUserService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();

var app = builder.Build();
app.UseMiddleware<MiddlewareExceptionHandler>();

#region Seed Fake Data
var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var loggerFactory = services.GetRequiredService<ILoggerFactory>();
var context = services.GetRequiredService<ApplicationDbContext>();

try
{
    await context.Database.MigrateAsync();
    await GenerateFakeData.SeedDataAsync(context, loggerFactory);
}
catch (Exception e)
{
    var logger = loggerFactory.CreateLogger<Program>();
    logger.LogError(e, "خطا در انجام مایگریش اولیه برای تزریق دیتای فیک");
}
#endregion


if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
}

app.UseStaticFiles();
app.UseCors("CorsPolicy");
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
await app.RunAsync();