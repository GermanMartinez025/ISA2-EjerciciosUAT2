using IRepositories.Repositories.HomesRepositories;
using Microsoft.EntityFrameworkCore;
using ModelInterface.Homes;
namespace Repositories.Homes;

public class HomeRepository: IHomeRepository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<AHome> _home;
    
    public HomeRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _home = dbContext.Set<AHome>();
    }
    
    public AHome Create(AHome home)
    {
        _home.Add(home);
        _dbContext.SaveChanges();
        
        return home;
    }

    public AHome? GetById(int id)
    { 
        AHome home = _home
            .Include(h => h.Owner)
            .Include(h => h.Members)
            .ThenInclude(m => m.AHomeUser)
            .ThenInclude(u => u.User)
            .Include(h => h.Devices)
            .ThenInclude(d => d.Device)
            .ThenInclude(d => d.Photos)
            .Include(d=>d.Rooms)
            .FirstOrDefault(h => h.Id == id);
        
        return home;
    }
    
    public List<AHome> GetAll()
    {
        return new List<AHome>(_home.ToList());
    }
    
    public List<AHome> GetMyHomes(int userId)
    {
        return _home
            .Include(h => h.Owner)
            .ThenInclude(o => o.User)
            .Include(h => h.Members)
            .ThenInclude(m => m.AHomeUser)
            .ThenInclude(u => u.User)
            .Where(h => h.Members.Any(m => m.AHomeUser.UserId == userId))
            .ToList();
    }
    
    public AHome Update (AHome home)
    {
        _home.Update(home);
        _dbContext.SaveChanges();

        return home;
    }
}