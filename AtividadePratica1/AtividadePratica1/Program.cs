using System.Text.Json;
using AtividadePratica1.ViewModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateTime.Now.AddDays(index),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
});

string productsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "products.json");
List<ProductGetViewModel> products = new List<ProductGetViewModel>();
for(int i=1;i<=10;i++){
    products.Add(new ProductGetViewModel($"Product {i}", (double)i*10, i*10));
}
var options = new JsonSerializerOptions
{
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};
var json = System.Text.Json.JsonSerializer.Serialize(products, options);
System.IO.File.WriteAllText(productsPath, json);

app.MapControllers();

app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}