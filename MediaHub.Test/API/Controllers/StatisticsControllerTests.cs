using System.Threading.Tasks;
using MediaHub.API.Controllers;
using MediaHub.DAL.FS.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace MediaHub.Test.API.Controllers
{
    public class StatisticsControllerTests
    {
        private readonly Mock<IStatisticsService> _mockStatisticsService;
        private readonly StatisticsController _controller;

        public StatisticsControllerTests()
        {
            _mockStatisticsService = new Mock<IStatisticsService>();
            _controller = new StatisticsController(_mockStatisticsService.Object);
        }

        [Fact]
        public async Task GetStatisticsOverview_ReturnsOkResult_WithStatisticsData()
        {
            // Arrange
            _mockStatisticsService.Setup(s => s.GetNumberOfSeries()).ReturnsAsync(10);
            _mockStatisticsService.Setup(s => s.GetNumberOfEpisodes()).ReturnsAsync(100);
            _mockStatisticsService.Setup(s => s.GetFileSystemUsage()).ReturnsAsync(500000);
            _mockStatisticsService.Setup(s => s.GetTotalSpaceAvailable()).ReturnsAsync(1000000);
            _mockStatisticsService.Setup(s => s.GetSpaceUsedByOtherApplications()).ReturnsAsync(200000);

            // Act
            var result = await _controller.GetStatisticsOverview();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var statistics = Assert.IsType<dynamic>(okResult.Value);
            Assert.Equal(10, statistics.SeriesCount);
            Assert.Equal(100, statistics.EpisodesCount);
            Assert.Equal(500000, statistics.FileSystemUsage);
            Assert.Equal(1000000, statistics.TotalSpaceAvailable);
            Assert.Equal(200000, statistics.SpaceUsedByOtherApplications);
        }

        [Fact]
        public async Task GetFolderSize_ReturnsOkResult_WithFolderSize()
        {
            // Arrange
            var path = "test/path";
            _mockStatisticsService.Setup(s => s.GetFolderSize(path)).ReturnsAsync(300000);

            // Act
            var result = await _controller.GetFolderSize(path);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var folderSize = Assert.IsType<dynamic>(okResult.Value);
            Assert.Equal(300000, folderSize.FolderSize);
        }
    }
}
