namespace ModelsAPI.Devices;

public class RequestCreateDevice
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ModelNumber { get; set; }
    public string Description { get; set; }
    public List<string> Photos { get; set; }
    public string? MainPhoto { get; set; }
    public string Type { get; set; }
    public bool? OutsideEnvironment { get; set; }
    public bool? MovementDetection { get; set; }
    public bool? PersonDetection { get; set; }
    
    public RequestCreateDevice()
    {
        
    }
}