using IRepositories.Repositories.SessionRepositories;
using Microsoft.EntityFrameworkCore;
using ModelInterface.Sessions;
using Models.Sessions;

namespace Repositories.SessionRepositories;

public class SessionRepository : ISessionRepository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<Session> _sessions;
    
    public SessionRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _sessions = dbContext.Set<Session>();
    }
    
    public ISession Create(ISession session)
    {
        _sessions.Add((Session)session);
        _dbContext.SaveChanges();

        return session;
    }

    public void Delete(ISession session)
    {
        _sessions.Remove((Session)session);
        
        _dbContext.SaveChanges();
    }

    public ISession? GetById(int id)
    {
        return _sessions.FirstOrDefault(x => x.Id == id);
    }

    public List<ISession> GetAll()
    {
        return _sessions.Cast<ISession>().ToList();
    }

    public ISession? GetByToken(string token)
    {
        return _sessions.FirstOrDefault(x => x.Token.ToString() == token);
    }
    
}