namespace MediaHub.DAL.FS.Services;

public interface IMediaThumbnailService
{
    public byte[] GetThumbnail(string path);
    
    public Task ExtractThumbnail(string path);
    
    public void ExtractThumbnailsForMediaFolder();
}