namespace MediaHub.DAL.FS.Model;

public interface IMedia
{
    public string Name { get; set; }
    public string Path { get; set; }
    public MediaType Type { get; set; }
    
    public IEnumerable<IMedia> Children { get; set; }
}