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

    public MediaThumbnailService(RootPathService rootPath, ThumbnailPathService thumbnailPath) : this(rootPath,
        thumbnailPath,
        new FileSystem())
    {
    }

    public byte[]? GetThumbnail(string path)
    {
        string thumbnailPath = _thumbnailPath.CombineRootPath(path + ".png");
        return _fileSystem.File.Exists(thumbnailPath) ? _fileSystem.File.ReadAllBytes(thumbnailPath) : null;
    }

    public string? GetThumbnailPath(string path)
    {
        string thumbnailPath = _thumbnailPath.CombineRootPath(path + ".png");
        return _fileSystem.File.Exists(thumbnailPath) ? path : null;
    }

    public async Task ExtractThumbnail(string path)
    {
        var time = DateTime.Now;
        // Construct the full path of the media file
        string mediaFilePath = _rootPath.CombineRootPath(path);

        // Construct the output thumbnail file path
        string thumbnailFilePath = _thumbnailPath.CombineRootPath(path + ".png");

        // Extract the thumbnail halfway through the video
        var halfway = TimeSpan.FromSeconds(FFmpeg.GetMediaInfo(mediaFilePath).Result.Duration.TotalSeconds / 2);
        IConversion conversion =
            await FFmpeg.Conversions.FromSnippet.Snapshot(mediaFilePath, thumbnailFilePath, halfway);
        // Start the conversion
        await conversion.Start();

        Console.WriteLine($"Thumbnail extracted for {path}");
    }

    public void ExtractThumbnailsForMediaFolder()
    {
        var mediaFiles = _fileSystem.Directory.GetFiles(_rootPath.Path, "*.*", SearchOption.AllDirectories)
            .Select(_rootPath.StripRootPath)
            .Where(file => file.EndsWith(".mp4") || file.EndsWith(".mkv"))
            .Where(file => !_fileSystem.File.Exists(_thumbnailPath.CombineRootPath(file + ".png")));
        
        foreach (var mediaFile in mediaFiles)
        {
            ExtractThumbnail(mediaFile).Wait();
        }
    }
}