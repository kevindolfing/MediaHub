using MediaHub.DAL.FS.Services;

namespace MediaHub.API.Service;

public class ThumbnailHostedService : IHostedService
{
    private readonly IMediaThumbnailService _thumbnailService;
    private CancellationTokenSource _cts;
    private const int Interval = 1000 * 60;


    public ThumbnailHostedService(IMediaThumbnailService thumbnailService)
    {
        _thumbnailService = thumbnailService;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        await Loop(_cts.Token);
    }

    private async Task Loop(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            Console.WriteLine("Extracting thumbnails");
            _thumbnailService.ExtractThumbnailsForMediaFolder();
            await Task.Delay(Interval, cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}