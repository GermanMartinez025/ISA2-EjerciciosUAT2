using IRepositories.Repositories.CompanyRepositories;
using ModelException;
using ModelInterface.Companys;
using ModelInterface.Users.UserType;
using Models.Company;
using Models.Users.UserTypes;
using ModelsAPI.Companies;
using ServicesInterfaces.Company;
using ServicesInterfaces.Users;

namespace Services.Companys;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _companyRepository;
    private readonly ICompanyOwnerService _companyOwnerService;
    
    public CompanyService(ICompanyRepository companyRepository, ICompanyOwnerService companyOwnerService)
    {
        _companyRepository = companyRepository;
        _companyOwnerService = companyOwnerService;
    }

    public ACompany AddCompanyToOwner(int companyOwnerId, RequestAddCompanyToOwner request)
    {
        CompanyOwner companyOwner = (CompanyOwner)_companyOwnerService.GetById(companyOwnerId);

        ValidateCompanyOwnerNotAssigned(companyOwner);
        
        Company company = new Company(request.Rut, request.Name, request.LogoType, companyOwner, request.ValidationType);
        
        _companyRepository.Create(company);
        
        _companyOwnerService.AddCompanyToCompanyOwner(companyOwner, company);

        return company;
    }
    
    public ACompany? GetCompany(int id)
    {
        ValidateIdIsGreaterThanZero(id);

        Company? company = _companyRepository.GetById(id);

        ValidateIdExits(company, id);

        return company;
    }

    public List<ACompany>? GetAllCompanys()
    {
        return _companyRepository.GetAll()
            .Cast<ACompany>()
            .ToList();
    }
    
    public List<ACompany> GetCompanies(int page, int pageSize, Dictionary<string, object>? filters)
    {
        return _companyRepository.GetPaginated(page, pageSize, filters)
            .Select(company => (ACompany)company)
            .ToList();
    }

    public int GetAmountOfCompanies(Dictionary<string, object>? filters)
    {
        return _companyRepository.Count(filters);
    }

    public ICompany GetMyCompany(int companyOwnerId)
    {
        ACompanyOwner companyOwner = _companyOwnerService.GetById(companyOwnerId);
        
        if (!companyOwner.HasCompanyAssigned)
        {
            throw new ConflictException("You don't have a company assigned");
        }

        var companyId = (int)companyOwner.CompanyId!;
        
        Company company = _companyRepository.GetById(companyId);
        

        return company;
    }
    
    public ACompany UpdateCompany(int companyId, int companyOwnerId, string newValidationType)
    {
        var company = GetCompany(companyId);
        ValidateCompanyOwner(companyOwnerId, company);
        company.ValidationType = newValidationType;
        _companyRepository.Update((Company)company);
        
        return company;
    }
    
    public void ValidateCompanyOwner(int companyOwnerId, ACompany company)
    {
      
        if (company.CompanyOwnerId != companyOwnerId)
        {
            throw new ConflictException("You are not the owner of this company");
        }
    }
  

    private void ValidateCompanyOwnerNotAssigned(ACompanyOwner companyOwner)
    {
        if (companyOwner.HasCompanyAssigned)
        {
            throw new ConflictException("The company owner already has a company assigned.");
        }
    }
    
    private void ValidateIdExits(Company company, int id)
    {
        if (company == null)
        {
            throw new NotFoundException($"The requested company was not found with ID {id}.");
        }
        if (company.Id != id)
        {
            throw new NotFoundException($"The provided ID does not match the company's ID.");
        }
    }
    
    private void ValidateIdIsGreaterThanZero(int id)
    {
        if (id <= 0)
        {
            throw new ConflictException("Id must be greater than zero.");
        }
    }
    
}