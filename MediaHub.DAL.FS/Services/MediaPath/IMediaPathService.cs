namespace MediaHub.DAL.FS.Services.MediaPath;

public interface IMediaPathService
{
    string StripRootPath(string path);
    string CombineRootPath(string path);
}