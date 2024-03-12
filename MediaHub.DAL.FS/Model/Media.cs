namespace MediaHub.DAL.FS.Model;

public class Media : IMedia
{
    public string Name { get; set; }
    public string Path { get; set; }
    public IEnumerable<IMedia> Children { get; set; }
}