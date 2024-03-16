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
            new MediaFolder { Name = "folder1" },
            new Media { Name = "file1.txt" },
            new Media { Name = "file2.txt" }
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
            new MediaFolder { Name = "folder1" },
            new Media { Name = "file1.txt" },
            new Media { Name = "file2.txt" }
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