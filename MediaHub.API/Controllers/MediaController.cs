using MediaHub.DAL.FS.Model;
using MediaHub.DAL.FS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

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
    public IEnumerable<IMedia> GetMedia([FromQuery] string? path)
    {
        return string.IsNullOrEmpty(path) ? _mediaService.GetMedia() : _mediaService.GetMedia(path);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Route("file")]
    public IActionResult GetMediaFile([FromQuery] string path)
    {
        var file = _mediaService.GetMediaFile(path);
        if (file == null)
        {
            return NotFound();
        }
        
        //read stream
        new FileExtensionContentTypeProvider().TryGetContentType(file.Name, out var contentType);
        
        var result = new FileStreamResult(file.OpenRead(), contentType ?? "application/octet-stream");
        
        Response.Headers["Content-Disposition"] = "inline; filename=" + file.Name;
        
        return result;

        
    }
}