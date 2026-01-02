using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Built-in OpenAPI document generation (.NET 10)
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCors", policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod());
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseCors("DevCors");
}


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


app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
