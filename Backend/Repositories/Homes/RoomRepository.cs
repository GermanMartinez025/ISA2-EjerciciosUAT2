using IRepositories.Repositories.HomesRepositories;
using Microsoft.EntityFrameworkCore;
using ModelInterface.Homes;
using Models.Homes;

namespace Repositories.Homes;

public class RoomRepository : IRoomRepository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<Room> _rooms;

    public RoomRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _rooms = dbContext.Set<Room>();
    }

    public ARoom Create(ARoom entity)
    {
        _rooms.Add((Room)entity);
        _dbContext.SaveChanges();

        return entity;
    }

    public ARoom? GetById(int id)
    {
        return _rooms
            .Include(r => r.Home)
            .Include(r => r.Devices)
            .FirstOrDefault(r => r.Id == id);
        
    }

    public List<ARoom> GetAll()
    {
        return new List<ARoom>(_rooms.ToList());
    }

    public void Delete(ARoom room)
    {
        _rooms.Remove((Room)room);
        _dbContext.SaveChanges();
    }
}