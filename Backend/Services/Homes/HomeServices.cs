using IRepositories.Repositories.HomesRepositories;
using ModelException;
using ServicesInterfaces.Homes;
using ModelInterface.Homes;
using ModelInterface.Users;
using ModelInterface.Users.UserType;
using Models.Homes;
using Models.Users.UserTypes;
using ModelsAPI.Homes;
using ServicesInterfaces.Users;

namespace Services.Homes;

public class HomeService : IHomeServices
{
    private IHomeUserService _homeUserServices;
    private readonly IHomeRepository _homeRepository;

    public HomeService(IHomeRepository homeRepository, IHomeUserService homeUserServices)
    {
        _homeRepository = homeRepository;
        _homeUserServices = homeUserServices;
    }

    public AHome AddHome(RequestCreateHome request, int userId)
    {
        AHomeUser aHomeUser = _homeUserServices.GetHomeUser(userId);
        var home = new Home(request.Address.MainStreet, request.Address.DoorNumber, request.Name, request.Geolocation.Latitude, request.Geolocation.Longitude, request.MaxMembers, aHomeUser);
        
        _homeRepository.Create(home);
        return home;
    }

    public bool IsOwner(int homeUserHomeId, int homeUserUserId)
    {
        AHome home = GetHome(homeUserHomeId);
        return home.HomeOwnerId == homeUserUserId;
    }
    
    public AHome GetHome(int id)
    {
        ValidateIdIsGreaterThanZero(id);
        
        return _homeRepository.GetById(id);
    }

    public List<AHome> GetAllHomes()
    {
        return _homeRepository.GetAll();
    }
    
    public AHome Update(AHome home)
    {
        ValidateHomeNotNull(home);
        
        return _homeRepository.Update(home);
    }

    public void AddRoomToHome(AHome home, ARoom room)
    {
        ValidateRoomNotNull(room);
        ValidateHomeNotNull(home);
        
        home.Rooms.Add(room);
    }
    
    private void ValidateIdIsGreaterThanZero(int id)
    {
        if (id <= 0)
        {
            throw new BadRequestException("Id must be greater than zero.");
        }
    }
    private void ValidateHomeNotNull(AHome home){
        if (home == null)
        {
            throw new BadRequestException("Home cannot be null.");
        }
    }
    
    private void ValidateRoomNotNull(ARoom room)
    {
        if (room == null)
        {
            throw new BadRequestException("Room cannot be null.");
        }
    }
    
    public List<AHome> GetMyHomes(int userId)
    {
        return _homeRepository.GetMyHomes(userId);
    }
}