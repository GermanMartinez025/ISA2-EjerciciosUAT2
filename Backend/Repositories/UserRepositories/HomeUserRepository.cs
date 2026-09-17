using IRepositories.Repositories.UserRepositories;
using Microsoft.EntityFrameworkCore;
using ModelInterface.Users;
using ModelInterface.Users.UserType;
using Models.Users.UserTypes;

namespace Repositories.UserRepositories;

public class HomeUserRepository : IHomeUserRepository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<HomeUser> _homeUsers;
    private IHomeUserRepository _homeUserRepositoryImplementation;

    public HomeUserRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _homeUsers = dbContext.Set<HomeUser>();
    }

    public AHomeUser? GetById(int id)
    {
       return _homeUsers.Include(hm => hm.User).
           FirstOrDefault(hm => hm.Id == id);
    }

    public List<AHomeUser> GetAll()
    {
        return new List<AHomeUser>(_homeUsers.ToList());
    }
    
    public AHomeUser? GetByEmail(string email)
    {
        return _homeUsers.Include(hm => hm.User)
            .FirstOrDefault(hm => hm.User.Email == email);
    }
    
    public AHomeUser Update(AHomeUser homeUser)
    {
        _homeUsers.Update((HomeUser)homeUser);
        _dbContext.SaveChanges();

        return homeUser;
    }
    
}