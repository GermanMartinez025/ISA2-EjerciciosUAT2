namespace Models.Importers;

public class ImportedDevice 
{
    
    public string Id { get; set; }
    public string Type { get; set; }
    public string Name { get; set; }
    public string Model { get; set; }
    public List<Photo> Photos { get; set; }
    
    public string MainPhoto { get; set; }
    public bool? PersonDetection { get; set; }
    public bool? MovementDetection { get; set; }
    public bool? OutsideEnvironment { get; set; }

    public ImportedDevice()
    {
        
    }

    public ImportedDevice(string id, string type, string name, string model, List<Photo> photos, bool? personDetection,
        bool? movementDetection) : this(id, type, name, model, photos, personDetection, movementDetection, null)
    {
        
    }
    public ImportedDevice(string id, string type, string name, string model, List<Photo> photos, bool? personDetection, bool? movementDetection, bool? outsideEnvironment)
    {
        Id = id;
        Type = type;
        Name = name;
        Model = model;
        Photos = photos;
        PersonDetection = personDetection;
        MovementDetection = movementDetection;
        MainPhoto = photos.FirstOrDefault(p => p.IsPrincipal)?.Path;
        OutsideEnvironment = outsideEnvironment;
    }
    
}