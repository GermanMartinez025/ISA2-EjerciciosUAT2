namespace IRepositories.CRUD;

public interface ICreateRepository<T>
{
    public T Create (T entity);
}
