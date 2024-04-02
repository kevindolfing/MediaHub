using MediaHub.DAL.FS.Model;
using MediaHub.DAL.FS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace MediaHub.API.Controllers;

[ApiController]
[Route("[controller]")]
public class MediaController : ControllerBase
{
    private readonly ILogger<MediaController> _logger;
    private readonly IMediaService _mediaService;
    private readonly IMediaThumbnailService _mediaThumbnailService;

    public MediaController(ILogger<MediaController> logger, IMediaService mediaService, IMediaThumbnailService mediaThumbnailService)
    {
        _logger = logger;
        _mediaService = mediaService;
        _mediaThumbnailService = mediaThumbnailService;
    }

    [HttpGet]
    [Authorize("read:media")]
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
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Route("thumbnail")]
    public IActionResult GetThumbnail([FromQuery] string path)
    {
        var thumbnail = _mediaThumbnailService.GetThumbnail(path);
        if (thumbnail == null)
        {
            return NotFound();
        }
        
        return File(thumbnail, "image/png");
    }
}