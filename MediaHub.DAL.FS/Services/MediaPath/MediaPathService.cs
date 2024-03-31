using System.IO.Abstractions;

namespace MediaHub.DAL.FS.Services.MediaPath;

public abstract class MediaPathService : IMediaPathService
{
    public string Path { get; private set; }
    private readonly IFileSystem _fileSystem;
    
    public MediaPathService(string path, IFileSystem fileSystem)
    {
        Path = path;
        _fileSystem = fileSystem;
        
        if (!_fileSystem.Directory.Exists(Path))
        {
            Console.WriteLine($"Path {Path} does not exist. Creating it.");
            _fileSystem.Directory.CreateDirectory(Path);
        }
    }
    
    public MediaPathService(string rootPath) : this(rootPath, new FileSystem())
    {
    }
    
    public string StripRootPath(string path)
    {
        string newPath = path.Replace(Path, "");
        newPath = newPath.StartsWith(_fileSystem.Path.DirectorySeparatorChar) ? newPath[1..] : newPath;
        return newPath.Replace("\\", "/");
    }

    public string CombineRootPath(string path)
    {
        return _fileSystem.Path.Combine(Path, path);
    }
    
    public string GetAbsolutePath()
    {
        return _fileSystem.Path.GetFullPath(CombineRootPath(Path));
    }
}