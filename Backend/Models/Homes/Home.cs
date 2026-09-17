using System.Reflection.Metadata;
using ModelInterface.Devices;
using ModelInterface.Homes;
using ModelInterface.Notifications;
using ModelInterface.Users;
using ModelInterface.Users.UserType;
using Models.Notifications;
using Models.Users.UserTypes;

namespace Models.Homes;

public class Home : AHome
{
 
    public Home()
    {
        
    }

    public Home(string mainStreet, int doorNumber, string name, double latitude, double longitude, int maxMembers,
        AHomeUser owner) : base(mainStreet, doorNumber, name, latitude, longitude, maxMembers, owner)
    {
        Members.Add(new HomeMember(this, owner));
        Users.Add(owner);
        
    }

    public override void NotifyMembers(AHomeDevice device, EventType eventType)
    {
        foreach (var member in Members)
        {
            if (member.Notifiable)
            {
                Notification notification = new Notification(eventType, device, member);
                member.AddNotification(notification);
            }
        }
    }
}