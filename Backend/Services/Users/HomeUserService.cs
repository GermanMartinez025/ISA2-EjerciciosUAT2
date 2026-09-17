using IRepositories.Repositories.UserRepositories;
using ModelException;
using ModelInterface.Users;
using ModelInterface.Users.UserType;
using ServicesInterfaces.Users;

namespace Services.Users;

public class HomeUserService : IHomeUserService
{
    private readonly IHomeUserRepository _homeUserRepository;
    
    public HomeUserService(IHomeUserRepository homeUserRepository)
    {
        _homeUserRepository = homeUserRepository;
    }

    public AHomeUser GetHomeUser(int id)
    {
        AHomeUser? user = _homeUserRepository.GetById(id);
        
        ValidateHomeUserExists(user);

        return user;
    }

    public List<AHomeUser> GetAllHomeUsers()
    {
        return _homeUserRepository.GetAll();
    }

    public AHomeUser GetHomeUserByEmail(string email)
    {
        AHomeUser? user = _homeUserRepository.GetByEmail(email);
        
        ValidateHomeUserExists(user);

        return user;
    }
    
    public void Update(AHomeUser user)
    {
        ValidateHomeUserExists(user);
        
        _homeUserRepository.Update(user);
    }
    
    private void ValidateHomeUserExists(AHomeUser homeUser)
    {
        if (homeUser == null)
        {
            throw new NotFoundException("Home member not found");
        }
    }
}