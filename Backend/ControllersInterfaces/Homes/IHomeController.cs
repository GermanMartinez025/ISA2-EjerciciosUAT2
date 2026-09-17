using ModelsAPI;
using ModelsAPI.Devices;
using ModelsAPI.Homes;
using ModelsAPI.Users;

namespace ControllersInterfaces.Homes;

public interface IHomeController
{
    public ResponseAddUserToHome AddUserToHome(int homeId, RequestAddUserToHome request);
    public ResponseUpdateHomeMember UpdateHomeMember(int homeId, int userId, RequestUpdateHomeMember request);
    public ResponseGetDevices GetDevices(int homeId, string token, string? rooomName);
    ResponseCreateHome CreateHome(RequestCreateHome request, string token);
    ResponseGetMembers GetMembers(int homeId, string token);
    ResponseHomeDevice CreateDevice(int homeId, int deviceId, string token);
    ResponseHomeDevice UpdateDevice(int homeId, Guid hardwareId, RequestUpdateHomeDevice request, string token);
    public ResponseGetHome UpDateName(int homeId, RequestUpDateHome request, string token);
    public ResponseCreateRoom CreateRoom(int homeId, RequestCreateRoom request, string token);
    ResponseHomeDevice AssignDeviceToRoom(int homeId, Guid hardwareId, int roomId, string token);
    ResponseGetHomes GetHomes(string token);
}