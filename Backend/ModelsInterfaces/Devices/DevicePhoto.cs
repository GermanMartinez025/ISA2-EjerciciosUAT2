namespace ModelInterface.Devices;

public  class  DevicePhoto
{
    public int Id { get; set; }
    public string Url { get; set; }
    public int DeviceId { get; set; }
    public ADevice Device { get; set; }

    public DevicePhoto()
    {
    }

    public DevicePhoto(string url)
    {
        Url = url;
    }
}