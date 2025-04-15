using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace Itenium.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    [HttpGet]
    public string Get()
    {
        return "Sunny";
    }

    [HttpPost]
    public async Task Post(TemperatureRequest request)
    {
        string json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        using var httpClient = new HttpClient();
        await httpClient.PostAsync("/post", content);
    }

    [HttpPost("PostAlt")]
    public async Task PostAlt(TemperatureRequest request)
    {
        using var httpClient = new HttpClient();
        await httpClient.PostAsJsonAsync("/post", request);
    }
}

public class TemperatureRequest
{
    public float AverageTemp { get; set; }
    public string Description { get; set; } = "";
}
