namespace MediaHub.DAL.FS.Model;

public class Media : IMedia
{
    public required string Name { get; set; }
    public required string Path { get; set; }
    public required MediaType Type { get; set; }
    public IEnumerable<IMedia> Children { get; set; }
}