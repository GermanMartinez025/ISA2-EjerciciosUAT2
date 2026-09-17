namespace IRepositories.CRUD;

public interface IGetPaginatedRepository<T>
{
    public List<T> GetPaginated(int page, int pageSize, Dictionary<string, object>? filters);
    
    public int Count(Dictionary<string, object>? filters);
}