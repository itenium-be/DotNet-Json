using Microsoft.AspNetCore.Mvc;

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
}
