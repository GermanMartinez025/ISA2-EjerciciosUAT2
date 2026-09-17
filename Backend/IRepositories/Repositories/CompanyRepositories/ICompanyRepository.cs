using IRepositories.CRUD;
using Models.Company;

namespace IRepositories.Repositories.CompanyRepositories;

public interface ICompanyRepository : ICreateRepository<Company>, IGetRepository<Company>, IGetPaginatedRepository<Company>, IUpdateRepository<Company>
{
}