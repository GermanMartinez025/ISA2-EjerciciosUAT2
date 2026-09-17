namespace Models.Importers;

public class Photo
{
    public string Path { get; set; }
    public bool IsPrincipal { get; set; }
    
    public Photo (string path, bool isPrincipal)
    {
        Path = path;
        IsPrincipal = isPrincipal;
    }
}