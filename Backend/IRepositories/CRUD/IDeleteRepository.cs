using Models.Devices;

namespace IRepositories.CRUD;

public interface IDeleteRepository <T>
{
    public void Delete(T entity);
}