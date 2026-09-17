using IRepositories.Repositories.UserRepositories;
using ModelException;
using ModelInterface.Companys;
using ModelInterface.Users;
using ModelInterface.Users.UserType;
using Models.Users.UserTypes;
using ServicesInterfaces.Users;

namespace Services.Users;

public class CompanyOwnerService : ICompanyOwnerService
{
    private readonly ICompanyOwnerRepository _companyOwnerRepository;
    
    public CompanyOwnerService(ICompanyOwnerRepository companyOwnerRepository)
    {
        _companyOwnerRepository = companyOwnerRepository;
    }

    public ACompanyOwner GetById(int companyOwnerId)
    {
        ACompanyOwner companyOwner = _companyOwnerRepository.GetById(companyOwnerId);
        
        ValidateCompanyOwnerIsNotNull(companyOwner);

        return companyOwner;
    }
    
    public void AddCompanyToCompanyOwner(ACompanyOwner companyOwner, ACompany company)
    {
        ValidateCompanyOwnerIsNotNull(companyOwner);
        
        companyOwner.Company = company;
        
        _companyOwnerRepository.Update(companyOwner);
    }

    public void ValidateAssignedCompany(ACompanyOwner companyOwner)
    {
        if (!companyOwner.HasCompanyAssigned)
        {
            throw new ConflictException("Company's owner does not have a company assigned");
        }            
    }
    
    private void ValidateCompanyOwnerIsNotNull(ACompanyOwner companyOwner)
    {
        if ((CompanyOwner)companyOwner == null)
        {
            throw new NotFoundException("Companys owner not found");
        }
    }
}