using System.Threading.Tasks;

namespace MediaHub.DAL.FS.Services
{
    public interface IStatisticsService
    {
        Task<int> GetNumberOfSeries();
        Task<int> GetNumberOfEpisodes();
        Task<long> GetFileSystemUsage();
        Task<long> GetFolderSize(string path);
        Task<long> GetTotalSpaceAvailable();
        Task<long> GetSpaceUsedByOtherApplications();
    }
}
