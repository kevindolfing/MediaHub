using System.IO.Abstractions.TestingHelpers;
using MediaHub.DAL.FS.Model;
using MediaHub.DAL.FS.Services;

namespace MediaHub.Test.DAL.FS;

[TestClass]
public class MediaServiceTests
{
    [TestMethod]
    public void GetMedia_ReturnsMediaFoldersAndFiles_WhenDirectoryContainsBoth()
    {
        // Arrange
        string root = Path.Combine("root");
        string folder1 = Path.Combine(root, "folder1");
        string file1 = Path.Combine(root, "file1.txt");
        string file2 = Path.Combine(root, "file2.txt");
        var mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { folder1, new MockDirectoryData() },
            { file1, new MockFileData("file1") },
            { file2, new MockFileData("file2") }
        });
        var service = new MediaService(root, mockFileSystem);

        // Act
        List<IMedia> result = service.GetMedia().ToList();

        // Assert
        Assert.AreEqual(3, result.Count);
        Assert.IsTrue(result.Any(media => media.Name == "folder1" && media is MediaFolder));
        Assert.IsTrue(result.Any(media => media.Name == "file1.txt" && media is Media));
        Assert.IsTrue(result.Any(media => media.Name == "file2.txt" && media is Media));
    }

    [TestMethod]
    public void GetMedia_ReturnsEmpty_WhenDirectoryIsEmpty()
    {
        // Arrange
        var mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { @"c:\empty", new MockDirectoryData() }
        });
        var service = new MediaService(@"c:\empty", mockFileSystem);

        // Act
        var result = service.GetMedia();

        // Assert
        Assert.IsFalse(result.Any());
    }

    [TestMethod]
    [ExpectedException(typeof(DirectoryNotFoundException))]
    public void GetMedia_ThrowsDirectoryNotFoundException_WhenDirectoryDoesNotExist()
    {
        // Arrange
        var mockFileSystem = new MockFileSystem();
        var service = new MediaService(@"c:\nonexistent", mockFileSystem);

        // Act
        service.GetMedia();
    }
}