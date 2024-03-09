using Microsoft.AspNetCore.Mvc;

namespace MediaHub.API.Controllers;

[ApiController]
[Route("[controller]")]
public class MediaController : ControllerBase
{
    private readonly ILogger<MediaController> _logger;

    public MediaController(ILogger<MediaController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public string Get()
    {
        return "Hello World";
    }
}