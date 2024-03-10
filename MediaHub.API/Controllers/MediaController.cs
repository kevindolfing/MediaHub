using MediaHub.DAL.FS.Model;
using MediaHub.DAL.FS.Services;
using Microsoft.AspNetCore.Mvc;

namespace MediaHub.API.Controllers;

[ApiController]
[Route("[controller]")]
public class MediaController : ControllerBase
{
    private readonly ILogger<MediaController> _logger;
    private readonly IMediaService _mediaService;

    public MediaController(ILogger<MediaController> logger, IMediaService mediaService)
    {
        _logger = logger;
        _mediaService = mediaService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IEnumerable<IMedia> GetMedia()
    {
        return _mediaService.GetMedia();
    }
}