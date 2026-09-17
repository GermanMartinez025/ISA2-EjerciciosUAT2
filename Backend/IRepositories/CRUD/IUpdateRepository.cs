namespace IRepositories.CRUD;

public interface IUpdateRepository <T>
{
    public T Update (T entity);
}