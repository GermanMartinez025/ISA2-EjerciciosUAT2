namespace ModelsAPI.Users;

public class RequestUpdateHomeMember
{
    public bool? Notifiable { get; set; }
    public bool? ListDevices { get; set; }
    public bool? AddDevices { get; set; }
}