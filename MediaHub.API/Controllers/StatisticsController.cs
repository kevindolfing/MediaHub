using MediaHub.DAL.FS.Services;
using Microsoft.AspNetCore.Mvc;

namespace MediaHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatisticsController : ControllerBase
{
    private readonly IStatisticsService _statisticsService;

    public StatisticsController(IStatisticsService statisticsService)
    {
        _statisticsService = statisticsService;
    }

    [HttpGet("overview")]
    public async Task<IActionResult> GetStatisticsOverview()
    {
        var seriesCount = await _statisticsService.GetNumberOfSeries();
        var episodesCount = await _statisticsService.GetNumberOfEpisodes();
        var fileSystemUsage = await _statisticsService.GetFileSystemUsage();
        var totalSpaceAvailable = await _statisticsService.GetTotalSpaceAvailable();
        var spaceUsedByOtherApplications = await _statisticsService.GetSpaceUsedByOtherApplications();

        return Ok(new
        {
            SeriesCount = seriesCount,
            EpisodesCount = episodesCount,
            FileSystemUsage = fileSystemUsage,
            TotalSpaceAvailable = totalSpaceAvailable,
            SpaceUsedByOtherApplications = spaceUsedByOtherApplications
        });
    }

    [HttpGet("filesize")]
    public async Task<IActionResult> GetFolderSize([FromQuery] string path)
    {
        var folderSize = await _statisticsService.GetFolderSize(path);
        return Ok(new { FolderSize = folderSize });
    }
}
