using System.IO.Abstractions;
using MediaHub.DAL.FS.Services.MediaPath;
using Xabe.FFmpeg;

namespace MediaHub.DAL.FS.Services;

public class MediaThumbnailService : IMediaThumbnailService
{
    private readonly RootPathService _rootPath;
    private readonly ThumbnailPathService _thumbnailPath;
    private readonly IFileSystem _fileSystem;

    public MediaThumbnailService(RootPathService rootPath, ThumbnailPathService thumbnailPath, IFileSystem fileSystem)
    {
        _rootPath = rootPath;
        _thumbnailPath = thumbnailPath;
        _fileSystem = fileSystem;
    }

    public MediaThumbnailService(RootPathService rootPath, ThumbnailPathService thumbnailPath) : this(rootPath, thumbnailPath,
        new FileSystem())
    {
    }

    public byte[] GetThumbnail(string path)
    {
        throw new NotImplementedException();
    }

    public async Task ExtractThumbnail(string path)
    {
       // Construct the full path of the media file
       string mediaFilePath = _rootPath.CombineRootPath(path);

       // Construct the output thumbnail file path
       string thumbnailFilePath = _thumbnailPath.CombineRootPath(path + ".jpg");

       
       // Extract the thumbnail halfway through the video

       FFmpeg.GetMediaInfo(mediaFilePath).Wait();
       var halfway = TimeSpan.FromSeconds(FFmpeg.GetMediaInfo(mediaFilePath).Result.Duration.TotalSeconds / 2);
       IConversion conversion = await FFmpeg.Conversions.FromSnippet.Snapshot(mediaFilePath, thumbnailFilePath, halfway);

       // Start the conversion
       await conversion.Start();

       Console.WriteLine($"Thumbnail extracted for {path}");
       
    }

    public void ExtractThumbnailsForMediaFolder()
    {
        var mediaFiles = _fileSystem.Directory.GetFiles(_rootPath.Path, "*.*", SearchOption.AllDirectories)
            .Where(file => file.EndsWith(".mp4") || file.EndsWith(".mkv") );

        Parallel.ForEach(mediaFiles, (mediaFile) =>
        {
            ExtractThumbnail(_rootPath.StripRootPath(mediaFile)).Wait();
        });
    }
}