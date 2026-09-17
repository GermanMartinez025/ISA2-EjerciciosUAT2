using IRepositories.Repositories.CompanyRepositories;
using Microsoft.EntityFrameworkCore;
using Models.Company;

namespace Repositories.CompanyRepositories;

public class CompanyRepository: ICompanyRepository
{
    private readonly DbSet<Company> _company;
    private readonly DbContext _dbContext;
    
    public CompanyRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _company = dbContext.Set<Company>();
    }
    public Company Create(Company company)
    {
        _company.Add(company);
        _dbContext.SaveChanges();
        
        return company;
    }

    public Company? GetById(int id)
    {
        return _company.Include(c => c.Devices)
            .Include(c => c.CompanyOwner)
            .ThenInclude(co => co.User)
            .FirstOrDefault(c => c.Id == id);
    }

    public List<Company> GetAll()
    {
        return new List<Company>(_company
            .Include(c => c.CompanyOwner)
            .ThenInclude(co => co.User)
            .Include(c => c.Devices)
        );
    }

    public List<Company> GetPaginated(int page, int pageSize, Dictionary<string, object>? filters)
    {
        IQueryable<Company> companies = GetAll().AsQueryable();
        companies = ApplyFilters(companies, filters);
        
        return companies.Skip((page - 1) * pageSize).Take(pageSize).ToList();
    }

    public int Count(Dictionary<string, object>? filters)
    {
        IQueryable<Company> company = _company;
        company = ApplyFilters(company, filters);
        
        return company.Count();
    }
    
    private IQueryable<Company> ApplyFilters(IQueryable<Company> companies, Dictionary<string, object>? filters)
    {
        if (filters == null || filters.Count == 0) return companies;
        
        IQueryable<Company> filteredCompanies = companies;
        foreach (var filter in filters)
        {
            filteredCompanies = filter.Key switch
            {
                "companyName" => FilterByCompanyName(filteredCompanies, (List<string>) filter.Value),
                "ownerFullName" => FilterByOwnerCompanyName(filteredCompanies, (List<string>) filter.Value),
                _ => filteredCompanies
            };
        }
        return filteredCompanies;
    }
    
    private IQueryable<Company> FilterByCompanyName(IQueryable<Company> companies, List<string> companyNames)
    {
        if (companyNames.Count == 0) return companies;
        
        return companies.Where(c => companyNames.Contains(c.Name));
    }
    
    private IQueryable<Company> FilterByOwnerCompanyName(IQueryable<Company> companies, List<string> ownerCompanyNames)
    {
        if (ownerCompanyNames.Count == 0) return companies;
        
        return companies.Where(c => ownerCompanyNames.Contains(c.CompanyOwner.User.FirstName + " " + c.CompanyOwner.User.LastName));
    }

    public Company Update(Company entity)
    {
        _company.Update(entity);
        _dbContext.SaveChanges();
        
        return entity;
    }
}