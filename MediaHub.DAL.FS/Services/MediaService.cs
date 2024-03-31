using System.IO.Abstractions;
using MediaHub.DAL.FS.Model;
using MediaHub.DAL.FS.Services.MediaPath;

namespace MediaHub.DAL.FS.Services;

public class MediaService : IMediaService
{
    private readonly IMediaPathService _mediaPathService;
    private readonly IFileSystem _fileSystem;

    public MediaService(RootPathService mediaPathService, IFileSystem fileSystem)
    {
        _mediaPathService = mediaPathService;
        _fileSystem = fileSystem;
    }

    public MediaService(RootPathService mediaPathService) : this(mediaPathService, new FileSystem())
    {
    }

    public IEnumerable<IMedia> GetMedia()
    {
        return GetMedia("");
    }

    public IEnumerable<IMedia> GetMedia(string path)
    {
        return _fileSystem.Directory.GetFileSystemEntries(_mediaPathService.CombineRootPath(path)).Select(entry =>
                _fileSystem.Directory.Exists(entry)
                    ? new Media
                    {
                        Path = _mediaPathService.StripRootPath(entry), Name = _fileSystem.Path.GetFileName(entry),
                        Type = MediaType.DIRECTORY
                    }
                    : new Media
                    {
                        Path = _mediaPathService.StripRootPath(entry), Name = _fileSystem.Path.GetFileName(entry), Type = MediaType.FILE
                    } as IMedia)
            .OrderBy(it => it.Type)
            .ThenBy(it => it.ExtractNumericValueFromName())
            .ThenBy(it => it.Name);
    }

    public FileInfo? GetMediaFile(string path)
    {
        string fullPath = _mediaPathService.CombineRootPath(path);
        return _fileSystem.File.Exists(fullPath) ? new FileInfo(fullPath) : null;
    }
}