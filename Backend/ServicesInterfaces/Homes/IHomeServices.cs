using ModelInterface.Homes;
using ModelInterface.Users;
using ModelsAPI.Homes;

namespace ServicesInterfaces.Homes;

public interface IHomeServices
{
    public AHome AddHome(RequestCreateHome request, int userId);
    
    public AHome GetHome(int id);
    
    public List<AHome> GetAllHomes();
    
    public AHome Update(AHome home);
    
    bool IsOwner(int homeUserHomeId, int homeUserUserId);

    public void AddRoomToHome(AHome home, ARoom room);
    
    public List<AHome> GetMyHomes(int userId);
}