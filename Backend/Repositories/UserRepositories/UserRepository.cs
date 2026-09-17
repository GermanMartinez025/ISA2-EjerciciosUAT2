using IRepositories.Repositories.UserRepositories;
using Microsoft.EntityFrameworkCore;
using ModelInterface.Users;
using Models.Users;

namespace Repositories.UserRepositories;

public class UserRepository : IUserRepository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<User> _users;

    public UserRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _users = dbContext.Set<User>();
    }
    
    public AUser Create(AUser entity)
    {
        _users.Add((User)entity);
        _dbContext.SaveChanges();
        
        return entity;
    }

    public AUser? GetById(int id)
    {
        var user = _users
            .Include(u => u.Roles)
            .FirstOrDefault(u => u.Id == id);
        
        return user;
    }
    
    public AUser? GetByEmail(string email)
    {
        var user = _users.Include(u=>u.Roles).FirstOrDefault(u => u.Email == email);
        
        return user;
    }

    public List<AUser> GetAll()
    {
        return new List<AUser>(_users.Include(u => u.Roles).ToList());
    }

    public List<AUser> GetPaginated(int page, int pageSize, Dictionary<string, object>? filters)
    {
        IEnumerable<AUser> users = GetAll();
        users = ApplyFilters(users, filters);

        return users.Skip((page - 1) * pageSize).Take(pageSize).ToList();
    }

    public int Count()
    {
        return _users.Count();
    }
    
    public int Count(Dictionary<string, object>? filters)
    {
        IEnumerable<AUser> users = _users;
        users = ApplyFilters(users, filters);

        return users.Count();
    }

    public AUser? ValidateCredentials(string email, string password)
    {
        return _users.FirstOrDefault(u => u.Email == email && u.Password == password);
    }
    
    public AUser Update(AUser user)
    {
        _users.Update((User)user);
        _dbContext.SaveChanges();
        return user;
    }
    
    public void Delete(AUser user)
    {
        _users.Remove((User)user);
        _dbContext.SaveChanges();
    }
    
    private IEnumerable<AUser> ApplyFilters(IEnumerable<AUser> users, Dictionary<string, object>? filters)
    {
        if (filters == null || filters.Count == 0) return users;
        
        IEnumerable<AUser> filteredUsers = users;
        foreach (var filter in filters)
        {
            filteredUsers = filter.Key switch
            {
                "fullName" => FilterByFullName(filteredUsers, (List<string>)filter.Value),
                "role" => FilterByRole(filteredUsers, (List<string>)filter.Value),
                _ => filteredUsers
            };
        }

        return filteredUsers;
    }
    
    private IEnumerable<AUser> FilterByFullName(IEnumerable<AUser> users, List<string> fullName)
    {
        if (fullName.Count == 0) return users;

        return users.Where(u => fullName.Contains(u.FirstName + " " + u.LastName));
    }
    
    private IEnumerable<AUser> FilterByRole(IEnumerable<AUser> users, List<string> role)
    {
        if (role.Count == 0) return users;
        
        return users.Where(u => role.Any(u.HasRole));
    }
}