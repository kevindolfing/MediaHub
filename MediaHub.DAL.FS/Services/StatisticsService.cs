using System.IO.Abstractions;
using System.Threading.Tasks;
using MediaHub.DAL.FS.Services.MediaPath;

namespace MediaHub.DAL.FS.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IMediaService _mediaService;
        private readonly IFileSystem _fileSystem;
        private readonly RootPathService _rootPathService;

        public StatisticsService(IMediaService mediaService, IFileSystem fileSystem, RootPathService rootPathService)
        {
            _mediaService = mediaService;
            _fileSystem = fileSystem;
            _rootPathService = rootPathService;
        }

        public async Task<int> GetNumberOfSeries()
        {
            var media = _mediaService.GetMedia();
            return await Task.FromResult(media.Count(m => m.Type == Model.MediaType.DIRECTORY));
        }

        public async Task<int> GetNumberOfEpisodes()
        {
            var media = _mediaService.GetMedia();
            return await Task.FromResult(media.Count(m => m.Type == Model.MediaType.FILE));
        }

        public async Task<long> GetFileSystemUsage()
        {
            var media = _mediaService.GetMedia();
            long totalSize = 0;

            foreach (var item in media)
            {
                if (item.Type == Model.MediaType.FILE)
                {
                    var fileInfo = _fileSystem.FileInfo.FromFileName(_rootPathService.CombineRootPath(item.Path));
                    totalSize += fileInfo.Length;
                }
                else if (item.Type == Model.MediaType.DIRECTORY)
                {
                    totalSize += await GetFolderSize(item.Path);
                }
            }

            return await Task.FromResult(totalSize);
        }

        public async Task<long> GetFolderSize(string path)
        {
            long totalSize = 0;
            var absolutePath = _rootPathService.CombineRootPath(path);
            var directoryInfo = _fileSystem.DirectoryInfo.FromDirectoryName(absolutePath);

            foreach (var file in directoryInfo.GetFiles("*", SearchOption.AllDirectories))
            {
                totalSize += file.Length;
            }

            return await Task.FromResult(totalSize);
        }

        public async Task<long> GetTotalSpaceAvailable()
        {
            var driveInfo = _fileSystem.DriveInfo.GetDrives().FirstOrDefault(d => d.RootDirectory.FullName == _rootPathService.RootPath);
            return await Task.FromResult(driveInfo?.TotalFreeSpace ?? 0);
        }

        public async Task<long> GetSpaceUsedByOtherApplications()
        {
            var driveInfo = _fileSystem.DriveInfo.GetDrives().FirstOrDefault(d => d.RootDirectory.FullName == _rootPathService.RootPath);
            if (driveInfo == null)
            {
                return await Task.FromResult(0L);
            }

            var totalSpace = driveInfo.TotalSize;
            var freeSpace = driveInfo.TotalFreeSpace;
            var usedByMediaHub = await GetFileSystemUsage();

            var usedByOtherApplications = totalSpace - freeSpace - usedByMediaHub;
            return await Task.FromResult(usedByOtherApplications);
        }
    }
}
