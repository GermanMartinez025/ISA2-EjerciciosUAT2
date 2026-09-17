using ControllersInterfaces.Companies;
using ModelInterface.Companys;
using ServicesInterfaces.Company;
using Mapper;
using ModelsAPI.Companies;
using ServicesInterfaces.Sessions;

namespace Controllers.Companies;

public class CompanyController : ICompanyController
{
    private readonly ICompanyService _companyService;
    private readonly ISessionService _sessionService;
    
    public CompanyController(ICompanyService companyService, ISessionService sessionService)
    {
        _companyService = companyService;
        _sessionService = sessionService;
    }

    public ResponseGetComapanies GetCompanies(int page, int pageSize, List<string>? companyNames, List<string>? ownerFullNames)
    {
        Dictionary<string, object> filtersDictionary = new Dictionary<string, object>();
        if (companyNames != null) filtersDictionary.Add("companyName", companyNames);
        if (ownerFullNames != null) filtersDictionary.Add("ownerFullName", ownerFullNames);
        
        var companies = _companyService.GetCompanies(page, pageSize, filtersDictionary);
        
        var responseCompanies = MapCompanies(companies);   
        responseCompanies.TotalCompanies = _companyService.GetAmountOfCompanies(filtersDictionary);
        responseCompanies.actualPage = page;
        responseCompanies.totalPages = (int)Math.Ceiling((double)responseCompanies.TotalCompanies / pageSize);
        
        return responseCompanies;
    }
    
    private ResponseGetComapanies MapCompanies(List<ACompany> companies)
    {
        var responseUsers = new ResponseGetComapanies();
        var mapperCompany = new Mapper<ACompany, ResponseCompany>();
        
        foreach (var company in companies)
        {
            responseUsers.Companies.Add(mapperCompany.Convert(company));
        }
        
        return responseUsers;
    }
    public ResponseAddCompanyToOwner AddOneCompanyToOwner(string token, RequestAddCompanyToOwner request)
    {
        int companyOwnerId = _sessionService.GetUserIdFromSession(token);
        
        var company = _companyService.AddCompanyToOwner(companyOwnerId, request);

        return new ResponseAddCompanyToOwner(company);
    }

    public ResponseCompany GetMyCompany(string token)
    {
        int companyOwnerId = _sessionService.GetUserIdFromSession(token);
        
        var company = _companyService.GetMyCompany(companyOwnerId);
        return new ResponseCompany(company);
    }
    
    public ResponseCompany UpdateCompany(string token, int companyId, string newValidationType)
    {
        int companyOwnerId = _sessionService.GetUserIdFromSession(token);
        
        var company = _companyService.UpdateCompany(companyId, companyOwnerId, newValidationType);
        return new ResponseCompany(company);
    }
}