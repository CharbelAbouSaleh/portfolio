using Scalar.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Data;


var builder = WebApplication.CreateBuilder(args);

// -----------------------------For deploying APIs---------------------------------
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrWhiteSpace(port))
{
    // Render sets PORT. Binding 0.0.0.0 is required in container environments.
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
}
//----------------------------------------------------------------------------------

// Built-in OpenAPI document generation (.NET 10)
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

var corsOrigins = builder.Configuration["CORS_ORIGINS"]
                  ?? "http://localhost:5173";

var origins = corsOrigins
    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AppCors", policy =>
        policy.WithOrigins(origins)
              .AllowAnyHeader()
              .AllowAnyMethod());
});


var app = builder.Build();

app.UseCors("AppCors");


if (app.Environment.IsDevelopment())
{
    // OpenAPI JSON: /openapi/v1.json
    app.MapOpenApi();

    // Scalar UI: /scalar
    app.MapScalarApiReference();
}

// Keep HTTP for now
// app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild",
    "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast(
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();

    return forecast;
})
.WithName("GetWeatherForecast");


app.MapGet("/api/profile", () =>
{
    return Results.Ok(new
    {
        name = "Charbel Abou Saleh",
        title = "Data Analyst / BI Developer",
        location = "Lebanon"
    });
});


app.MapGet("/api/projects", async (AppDbContext db) =>
{
    var projects = await db.Projects
        .OrderByDescending(p => p.Id)
        .ToListAsync();

    return Results.Ok(projects);
});


app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
