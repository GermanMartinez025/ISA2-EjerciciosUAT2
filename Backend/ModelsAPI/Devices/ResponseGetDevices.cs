namespace ModelsAPI.Devices;

public class ResponseGetDevices
{
    public List<ResponseHomeDevice> Devices { get; set; } = new List<ResponseHomeDevice>();
}