using ModelInterface.Devices;
using ModelInterface.Homes;
using ModelInterface.Users;
using ModelInterface.Users.UserType;

namespace ServicesInterfaces.Users;

public interface IHomeMemberService
{
    AHomeMember Create(AHomeUser aHomeUser, AHome home);
    void AddUserToHome(int homeId, string email);
    void SetNotifiable(int homeId, int userId, bool notifiable);
    void SetListDevices(int homeId, int userId, bool listDevices);
    void SetAddDevices(int homeId, int userId, bool addDevices);
    void SetUpdateDevices(int homeId, int userId, bool updateDevices);
    void IsNotifiable(AHomeMember homeMember);
    void CanListDevices(AHomeMember homeMember);
    void CanAddDevices(AHomeMember homeMember);
    List<AHomeDevice> GetDevices(int homeId, int userId, string? roomName);
    List<AHomeMember> GetHomeMembers(int homeId, int userId);
    AHomeMember GetHomeMember(int homeId, int userId);
    void CanUpdateDevices(AHomeMember homeMember);
}