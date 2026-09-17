namespace ModelsAPI.Users;

public class ResponseUpdateHomeMember
{
    public int HomeId { get; set; }
    public int UserId { get; set; }
    public bool Notifiable { get; set; }
    public bool ListDevices { get; set; }
    public bool AddDevices { get; set; }
    
    public ResponseUpdateHomeMember()
    {
    }
    
    public ResponseUpdateHomeMember(int homeId, int userId, bool notifiable, bool listDevices, bool addDevices)
    {
        HomeId = homeId;
        UserId = userId;
        Notifiable = notifiable;
        ListDevices = listDevices;
        AddDevices = addDevices;
    }
    
    
}