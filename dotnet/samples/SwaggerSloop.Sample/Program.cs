using SwaggerSloop;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Sample API", Version = "v1" });
    c.SwaggerDoc("v2", new() { Title = "Sample API", Version = "v2" });
});

var app = builder.Build();

// Enable Swagger
app.UseSwagger();

// Use SwaggerSloop UI
app.UseSwaggerSloop(options =>
{
    options.DocumentTitle = "Sample API Documentation";
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
    options.SwaggerEndpoint("/swagger/v2/swagger.json", "API V2");
    options.DefaultTheme = SwaggerSloopTheme.Auto;
    options.PrimaryColor = "#5D87FF";
});

// Sample API endpoints
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/api/v1/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
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
.WithTags("Weather");

app.MapGet("/api/v1/users", () => new[] { new { Id = 1, Name = "John" }, new { Id = 2, Name = "Jane" } })
    .WithName("GetUsers")
    .WithTags("Users");

app.MapPost("/api/v1/users", (UserRequest user) => Results.Created($"/api/v1/users/{1}", new { Id = 1, user.Name, user.Email }))
    .WithName("CreateUser")
    .WithTags("Users");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

record UserRequest(string Name, string Email);
