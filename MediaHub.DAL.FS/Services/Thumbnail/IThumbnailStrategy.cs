namespace MediaHub.DAL.FS.Services.Thumbnail;

public interface IThumbnailStrategy
{
    public Task ExtractThumbnail(string path);
    IEnumerable<string> SupportedExtensions { get; }
}