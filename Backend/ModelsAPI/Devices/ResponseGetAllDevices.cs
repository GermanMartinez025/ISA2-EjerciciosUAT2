namespace ModelsAPI.Devices;

public class ResponseGetAllDevices
{
    public List<ResponseDevice> Devices { get; set; }
    public int TotalDevices { get; set; }
    public int actualPage { get; set; }
    public int totalPages { get; set; }
    public ResponseGetAllDevices()
    {
        Devices = new List<ResponseDevice>();
        TotalDevices = 0;
        actualPage = 0;
        totalPages = 0;
    }
}