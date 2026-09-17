using IRepositories.Repositories.HomesRepositories;
using ModelException;
using ModelInterface.Devices;
using ModelInterface.Homes;
using ModelInterface.Users;
using ModelInterface.Users.UserType;
using Models.Users.UserTypes;
using ServicesInterfaces.Devices;
using ServicesInterfaces.Homes;
using ServicesInterfaces.Users;

namespace Services.Users;

public class HomeMemberService : IHomeMemberService
{
    private IHomeServices _homeServices;
    private IHomeUserService _homeUserServices;
    public HomeMemberService(IHomeServices homeServices, IHomeUserService homeUserServices)
    {
        _homeServices = homeServices;
        _homeUserServices = homeUserServices;
    }
    
    public AHomeMember Create(AHomeUser aHomeUser, AHome home)
    {
        AHomeMember newHomeMember = new HomeMember(home, aHomeUser);
        return newHomeMember;
    }
    
    public void AddUserToHome(int homeId, string email)
    {
        AHome home = _homeServices.GetHome(homeId);
        AHomeUser aHomeUser = _homeUserServices.GetHomeUserByEmail(email);
        Validate (home, aHomeUser);
        AHomeMember newAHomeMember = Create(aHomeUser, home);
    
        home.Members.Add(newAHomeMember);
        _homeServices.Update(home);
    }
    
    public void SetNotifiable(int homeId, int userId, bool notifiable)
    {
        AHome home = _homeServices.GetHome(homeId);
        AHomeMember homeMember = GetHomeMember(home, userId);
        
        homeMember.Notifiable = notifiable;
        _homeServices.Update(home);
    }
    
    public void SetListDevices(int homeId, int userId, bool listDevices)
    {
        AHome home = _homeServices.GetHome(homeId);
        AHomeMember homeUser = GetHomeMember(home, userId);
        
        homeUser.ListDevices = listDevices;
        _homeServices.Update(home);
    }
    
    public void SetAddDevices(int homeId, int userId, bool addDevices)
    {
        AHome home = _homeServices.GetHome(homeId);
        AHomeMember homeUser = GetHomeMember(home, userId);
        
        homeUser.AddDevices = addDevices;
        _homeServices.Update(home);
    }

    public void SetUpdateDevices(int homeId, int userId, bool updateDevices)
    {
        AHome home = _homeServices.GetHome(homeId);
        AHomeMember homeUser = GetHomeMember(home, userId);
        
        homeUser.UpdateDevices = updateDevices;
        _homeServices.Update(home);
    }

    public List<AHomeDevice> GetDevices(int homeId, int userId, string? roomName)
    {
        AHome home = _homeServices.GetHome(homeId);
        AHomeUser homeUser = _homeUserServices.GetHomeUser(userId);
        
        if (!ValidateIsOwner(home, homeUser))
        {
            throw new ForbiddenException("User is not the owner of this home");
        }

        if (roomName == null)
        {
            return home.Devices;
        }
        ARoom room = ValidateRoom(home, roomName);
        
        return room.Devices;
    }

   public List<AHomeMember>? GetHomeMembers(int homeId, int userId)
    {
        AHome home = _homeServices.GetHome(homeId);
        AHomeUser homeUser = _homeUserServices.GetHomeUser(userId);

        if (!ValidateIsOwner(home, homeUser))
        {
            throw new ForbiddenException("User is not the owner of this home");
        }
        
        return home.Members;     
    }
    
    public AHomeMember GetHomeMember(int homeId, int userId)
    {
        AHome home = _homeServices.GetHome(homeId);
        AHomeMember homeUser = GetHomeMember(home, userId);
        
        return homeUser;
    }

    public void CanUpdateDevices(AHomeMember homeMember)
    {
        if(homeMember.UpdateDevices == false && !IsOwner(homeMember))
        {
            throw new ForbiddenException("User can't update devices");
        }
    }

    public void CanListDevices(AHomeMember homeUser)
    {
        if (homeUser.ListDevices == false && !IsOwner(homeUser))
        {
            throw new ForbiddenException("User can't list devices");
        }
    }
    
    public void CanAddDevices(AHomeMember homeUser)
    {
        if (homeUser.AddDevices == false && !IsOwner(homeUser))
        {
            throw new ForbiddenException("User can't add devices");
        }
    }
    
    public void IsNotifiable(AHomeMember homeUser)
    {
        if (homeUser.Notifiable == false)
        {
            throw new ConflictException("User can't receive notifications");
        }
    }
    
    private bool IsOwner(AHomeMember homeUser) => _homeServices.IsOwner(homeUser.HomeId, homeUser.UserId);
    
    private void Validate(AHome home, AHomeUser homeUser)
    {
        ValidateHome(home);
        ValidateUser(homeUser, home);
    }

    private void ValidateHome(AHome home)
    {
        if (home.Members.Count >= home.MaxMembers)
        {
            throw new ConflictException("Can't add any more members to this home");
        }
    }
    
    private void ValidateUser(AHomeUser aHomeUser, AHome home)
    {
        bool isUserAlreadyMember = home.Members.Any(m => m.UserId == aHomeUser.UserId);
        if (isUserAlreadyMember)
        {
            throw new ConflictException("HomeUser already member of this home");
        }
    }
    
    private void ValidateNotNullUser(AHomeMember? homeUser)
    {
        if (homeUser == null)
        {
            throw new NotFoundException("User not found");
        }
    }
    
    private AHomeMember GetHomeMember(AHome home, int userId)
    {
        AHomeMember? homeUser = home.Members.FirstOrDefault(mu => mu.UserId == userId);
        ValidateNotNullUser(homeUser);

        return homeUser;
    }
    
    private bool ValidateIsOwner(AHome home, AHomeUser homeUser)
    {
        return home.Owner == homeUser;
    }
    
    private ARoom ValidateRoom(AHome home, string? roomName)
    {
        ARoom room = home.Rooms.FirstOrDefault(r => r.Name == roomName);
        
        if (room == null)
        {
            throw new NotFoundException("Room not found");
        }
        if (room.Devices == null)
        {
            throw new ConflictException("Room has no devices");
        }
        return room;
    }
    
}