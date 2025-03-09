using Infrastructure;
using Infrastructure.Data;

using Microsoft.EntityFrameworkCore;

using WebApi;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddWebApi();
    builder.Services.AddInfrastructure(builder.Configuration);

    // Add services to the container.
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

WebApplication app = builder.Build();
{
    using (IServiceScope scope = app.Services.CreateScope())
    {
        IServiceProvider services = scope.ServiceProvider;

        AppDbContext context = services.GetRequiredService<AppDbContext>();
        await context.Database.MigrateAsync();
    }

    app.UseExceptionHandler();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    string[] summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    app.MapGet("/weatherforecast", () =>
        {
            WeatherForecast[] forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecast
                    (
                        DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        Random.Shared.Next(-20, 55),
                        summaries[Random.Shared.Next(summaries.Length)]
                    ))
                .ToArray();
            return forecast;
        })
        .WithName("GetWeatherForecast")
        .WithOpenApi();

    app.MapGet("/throw-exception", () =>
    {
        throw new Exception("yeah");
    });

    app.Run();
}

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF
    {
        get => 32 + (int)(TemperatureC / 0.5556);
    }
}

// ReSharper disable once ClassNeverInstantiated.Global
public partial class Program
{
}