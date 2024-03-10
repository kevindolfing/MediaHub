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
    }

    public MediaService(string rootPath) : this(rootPath, new FileSystem())
    {
    }

    public IEnumerable<IMedia> GetMedia()
    {
        var media = new List<IMedia>();
        string[] entries = _fileSystem.Directory.GetFileSystemEntries(_rootPath);
        //if directory add mediaFolder, if file add mediaFile
        foreach (string entry in entries)
        {
            if (_fileSystem.Directory.Exists(entry))
            {
                media.Add(new MediaFolder
                {
                    Path = entry,
                    Name = _fileSystem.Path.GetFileName(entry)
                });
            }
            else
            {
                media.Add(new Media
                {
                    Path = entry,
                    Name = _fileSystem.Path.GetFileName(entry)
                });
            }
        }

        return media;
    }
}