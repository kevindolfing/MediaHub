using System.IO.Abstractions;
using MediaHub.DAL.FS.Model;

namespace MediaHub.DAL.FS.Services;

public class MediaService : IMediaService
{
    private readonly string _rootPath;
    private readonly IFileSystem _fileSystem;

    public MediaService(string rootPath, IFileSystem fileSystem)
    {
        _rootPath = rootPath;
        _fileSystem = fileSystem;

        if (!_fileSystem.Directory.Exists(_rootPath))
        {
            Console.WriteLine($"File system root path {_rootPath} does not exist. Creating it.");
            _fileSystem.Directory.CreateDirectory(_rootPath);
        }
        
    }

    public MediaService(string rootPath) : this(rootPath, new FileSystem())
    {
    }

    public IEnumerable<IMedia> GetMedia()
    {
        return GetMedia(_rootPath);
    }
    
    public IEnumerable<IMedia> GetMedia(string path)
    {
        if (!path.Contains(_rootPath))
            throw new ArgumentException("Path is not in the root path");
        return _fileSystem.Directory.GetFileSystemEntries(path).Select(entry =>
            _fileSystem.Directory.Exists(entry)
                ? new MediaFolder { Path = entry, Name = _fileSystem.Path.GetFileName(entry) }
                : new Media { Path = entry, Name = _fileSystem.Path.GetFileName(entry) } as IMedia);
    }
}