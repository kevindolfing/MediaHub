using MediaHub.API.Controllers;
using MediaHub.DAL.FS.Model;
using MediaHub.DAL.FS.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace MediaHub.Test.API.Controllers;

[TestClass]
public class MediaControllerTests
{
    [TestMethod]
    public void GetMedia_ReturnsMedia_WhenServiceReturnsMedia()
    {
        // Arrange
        var media = new List<IMedia>
        {
            new Media { Name = "folder1", Path = "", Type = MediaType.DIRECTORY},
            new Media { Name = "file1.txt", Path = "", Type = MediaType.FILE },
            new Media { Name = "file2.txt", Path = "", Type = MediaType.FILE }
        };
        var mockService = new Mock<IMediaService>();
        mockService.Setup(service => service.GetMedia()).Returns(media);
        var controller = new MediaController(new Mock<ILogger<MediaController>>().Object, mockService.Object);

        // Act
        IEnumerable<IMedia> result = controller.GetMedia(null);

        // Assert
        Assert.AreEqual(media, result);
    }
    
    [TestMethod]
    public void GetMedia_ReturnsMedia_WhenServiceReturnsMediaForPath()
    {
        // Arrange
        var media = new List<IMedia>
        {
            new Media { Name = "folder1", Path = "", Type = MediaType.DIRECTORY},
            new Media { Name = "file1.txt", Path = "", Type = MediaType.FILE },
            new Media { Name = "file2.txt", Path = "", Type = MediaType.FILE }
        };
        var mockService = new Mock<IMediaService>();
        mockService.Setup(service => service.GetMedia("path")).Returns(media);
        var controller = new MediaController(new Mock<ILogger<MediaController>>().Object, mockService.Object);

        // Act
        IEnumerable<IMedia> result = controller.GetMedia("path");

        // Assert
        Assert.AreEqual(media, result);
    }
}