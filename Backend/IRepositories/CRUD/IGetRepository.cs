namespace IRepositories.CRUD;

public interface IGetRepository<T, TId>
{
    public T GetById(TId id);
    public List<T> GetAll();
}

public interface IGetRepository<T> : IGetRepository<T, int>
{
}