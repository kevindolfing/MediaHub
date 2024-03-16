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
        return GetMedia("");
    }
    
    public IEnumerable<IMedia> GetMedia(string path)
    {
        return _fileSystem.Directory.GetFileSystemEntries(CombineRootPath(path)).Select(entry =>
            _fileSystem.Directory.Exists(entry)
                ? new MediaFolder { Path = StripRootPath(entry), Name = _fileSystem.Path.GetFileName(entry) }
                : new Media { Path = StripRootPath(entry), Name = _fileSystem.Path.GetFileName(entry) } as IMedia);
    }
    
    public string StripRootPath(string path)
    {
        string newPath = path.Replace(_rootPath, "");
        newPath = newPath.StartsWith(_fileSystem.Path.DirectorySeparatorChar) ? newPath.Substring(1) : newPath;
        return newPath.Replace("\\", "/");
    }
    
    public string CombineRootPath(string path)
    {
        return _fileSystem.Path.Combine(_rootPath, path);
    }
}