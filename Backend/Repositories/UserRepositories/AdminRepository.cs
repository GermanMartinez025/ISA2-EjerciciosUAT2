using IRepositories.Repositories.UserRepositories;
using Microsoft.EntityFrameworkCore;
using ModelInterface.Users;
using ModelInterface.Users.UserType;
using Models.Users.UserTypes;

namespace Repositories.UserRepositories;

public class AdminRepository : IAdminRepository 
{
    private readonly DbContext _dbContext;
    private readonly DbSet<Admin> _admins;
    
    
    public AdminRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _admins = dbContext.Set<Admin>();
    }
    
    public AAdmin? GetById(int id)
    {
        return _admins.FirstOrDefault(x => x.UserId == id);
    }
    
    public List<AAdmin> GetAll()
    {
        return _admins.Cast<AAdmin>().ToList();
    }

    public void Delete(AAdmin entity)
    {
        _admins.Remove((Admin)entity);
        _dbContext.SaveChanges();
    }

   
}