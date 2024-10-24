using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using MediaHub.DAL.FS.Model;
using MediaHub.DAL.FS.Services;
using MediaHub.DAL.FS.Services.MediaPath;
using Moq;
using Xunit;

namespace MediaHub.Test.DAL.FS
{
    public class StatisticsServiceTests
    {
        private readonly Mock<IMediaService> _mockMediaService;
        private readonly Mock<IFileSystem> _mockFileSystem;
        private readonly Mock<RootPathService> _mockRootPathService;
        private readonly StatisticsService _statisticsService;

        public StatisticsServiceTests()
        {
            _mockMediaService = new Mock<IMediaService>();
            _mockFileSystem = new Mock<IFileSystem>();
            _mockRootPathService = new Mock<RootPathService>("rootPath");
            _statisticsService = new StatisticsService(_mockMediaService.Object, _mockFileSystem.Object, _mockRootPathService.Object);
        }

        [Fact]
        public async Task GetNumberOfSeries_ReturnsCorrectCount()
        {
            // Arrange
            var mediaList = new List<IMedia>
            {
                new Media { Type = MediaType.DIRECTORY },
                new Media { Type = MediaType.DIRECTORY },
                new Media { Type = MediaType.FILE }
            };
            _mockMediaService.Setup(s => s.GetMedia()).Returns(mediaList);

            // Act
            var result = await _statisticsService.GetNumberOfSeries();

            // Assert
            Assert.Equal(2, result);
        }

        [Fact]
        public async Task GetNumberOfEpisodes_ReturnsCorrectCount()
        {
            // Arrange
            var mediaList = new List<IMedia>
            {
                new Media { Type = MediaType.DIRECTORY },
                new Media { Type = MediaType.FILE },
                new Media { Type = MediaType.FILE }
            };
            _mockMediaService.Setup(s => s.GetMedia()).Returns(mediaList);

            // Act
            var result = await _statisticsService.GetNumberOfEpisodes();

            // Assert
            Assert.Equal(2, result);
        }

        [Fact]
        public async Task GetFileSystemUsage_ReturnsCorrectSize()
        {
            // Arrange
            var mediaList = new List<IMedia>
            {
                new Media { Type = MediaType.FILE, Path = "file1" },
                new Media { Type = MediaType.DIRECTORY, Path = "dir1" }
            };
            _mockMediaService.Setup(s => s.GetMedia()).Returns(mediaList);
            _mockFileSystem.Setup(fs => fs.FileInfo.FromFileName(It.IsAny<string>()).Length).Returns(100);
            _mockRootPathService.Setup(rps => rps.CombineRootPath(It.IsAny<string>())).Returns<string>(p => p);
            _mockFileSystem.Setup(fs => fs.DirectoryInfo.FromDirectoryName(It.IsAny<string>()).GetFiles("*", SearchOption.AllDirectories)).Returns(new FileInfoBase[0]);

            // Act
            var result = await _statisticsService.GetFileSystemUsage();

            // Assert
            Assert.Equal(100, result);
        }

        [Fact]
        public async Task GetFolderSize_ReturnsCorrectSize()
        {
            // Arrange
            var directoryInfoMock = new Mock<DirectoryInfoBase>();
            directoryInfoMock.Setup(d => d.GetFiles("*", SearchOption.AllDirectories)).Returns(new FileInfoBase[]
            {
                new Mock<FileInfoBase>().Object,
                new Mock<FileInfoBase>().Object
            });
            _mockFileSystem.Setup(fs => fs.DirectoryInfo.FromDirectoryName(It.IsAny<string>())).Returns(directoryInfoMock.Object);
            _mockRootPathService.Setup(rps => rps.CombineRootPath(It.IsAny<string>())).Returns<string>(p => p);

            // Act
            var result = await _statisticsService.GetFolderSize("testPath");

            // Assert
            Assert.Equal(2, result);
        }

        [Fact]
        public async Task GetTotalSpaceAvailable_ReturnsCorrectSpace()
        {
            // Arrange
            var driveInfoMock = new Mock<DriveInfoBase>();
            driveInfoMock.Setup(d => d.TotalFreeSpace).Returns(1000);
            _mockFileSystem.Setup(fs => fs.DriveInfo.GetDrives()).Returns(new[] { driveInfoMock.Object });

            // Act
            var result = await _statisticsService.GetTotalSpaceAvailable();

            // Assert
            Assert.Equal(1000, result);
        }

        [Fact]
        public async Task GetSpaceUsedByOtherApplications_ReturnsCorrectSpace()
        {
            // Arrange
            var driveInfoMock = new Mock<DriveInfoBase>();
            driveInfoMock.Setup(d => d.TotalSize).Returns(2000);
            driveInfoMock.Setup(d => d.TotalFreeSpace).Returns(1000);
            _mockFileSystem.Setup(fs => fs.DriveInfo.GetDrives()).Returns(new[] { driveInfoMock.Object });
            _mockMediaService.Setup(s => s.GetMedia()).Returns(new List<IMedia>());
            _mockFileSystem.Setup(fs => fs.FileInfo.FromFileName(It.IsAny<string>()).Length).Returns(500);

            // Act
            var result = await _statisticsService.GetSpaceUsedByOtherApplications();

            // Assert
            Assert.Equal(500, result);
        }
    }
}
