using DataAccess;
using IRepositories;
using Models;

namespace AbstractRepositories;

public interface class ISensorRepository : ICreateRepository<Sensor>, IGetRepository<Sensor>
{
    public void Create(Sensor entity);

    public Sensor GetById(Guid id);

    public List<Sensor> GetAll();
}