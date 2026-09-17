using IRepositories.Repositories.UserRepositories;
using Microsoft.EntityFrameworkCore;
using ModelInterface.Users;
using ModelInterface.Users.UserType;
using Models.Users.UserTypes;

namespace Repositories.UserRepositories;

public class CompanyOwnerRepository : ICompanyOwnerRepository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<ACompanyOwner> _companyOwners;
    public CompanyOwnerRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _companyOwners = dbContext.Set<ACompanyOwner>();
    }

    public ACompanyOwner? GetById(int id)
    {
        return _companyOwners
            .Include(co => co.Company)
            .FirstOrDefault(x => x.UserId == id);
    }
    
    public List<ACompanyOwner> GetAll()
    {
        return _companyOwners.Cast<ACompanyOwner>().ToList();
    }
    
    public ACompanyOwner Update(ACompanyOwner companyOwner)
    {
        _companyOwners.Update((CompanyOwner)companyOwner);
        _dbContext.SaveChanges();

        return companyOwner;
    }
}