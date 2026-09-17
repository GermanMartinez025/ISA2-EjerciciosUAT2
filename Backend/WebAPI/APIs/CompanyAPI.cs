using ControllersInterfaces.Companies;
using ControllersInterfaces.Importers;
using ControllersInterfaces.Users;
using Microsoft.AspNetCore.Mvc;
using ModelsAPI;
using ModelsAPI.Companies;
using ModelsAPI.Importers;
using WebAPI.Filters;

namespace WebAPI.APIs;

[ApiController]
[Route("api/companies")]
[ExceptionFilter]
public class CompanyAPI : ControllerBase
{
    private readonly ICompanyController _companyController;
    private readonly IUserController _userController;
    private readonly IImporterController _importController;


    public CompanyAPI(ICompanyController companyController, IImporterController importController)
    {
        _companyController = companyController;
        _importController = importController;
    }
    
    [AuthorizationFilter("Admin")]
    [HttpGet]
    public GenericResponse GetCompanies(int page, int pageSize, [FromQuery] List<string>? companyNames, [FromQuery] List<string>? ownerFullNames)
    {
        var response = _companyController.GetCompanies(page, pageSize, companyNames, ownerFullNames);
        var genericResponse = new GenericResponse();
        genericResponse.Data = response;
        genericResponse.ExecutionSuccessful = true;
        genericResponse.Message = "Companys retrieved successfully";
        return genericResponse;
    }

    [HttpPost]
    public GenericResponse AddOneCompanyToOwner(RequestAddCompanyToOwner request)
    {
        var token = Request.Headers["Authorization"];
        var response = _companyController.AddOneCompanyToOwner(token, request);
        var genericResponse = new GenericResponse();
        genericResponse.Data = response;
        genericResponse.ExecutionSuccessful = true;
        genericResponse.Message = "Companys added successfully";
        return genericResponse;
    }
    
    [HttpGet("me")]
    [AuthorizationFilter("CompanyOwner")]
    public GenericResponse GetMyCompany()
    {
        var token = Request.Headers["Authorization"];
        var response = _companyController.GetMyCompany(token);
        var genericResponse = new GenericResponse();
        genericResponse.Data = response;
        genericResponse.ExecutionSuccessful = true;
        genericResponse.Message = "Company retrieved successfully";
        return genericResponse;
    }
    
    [HttpPut("{companyId}")]
    [AuthorizationFilter("CompanyOwner")]
    public GenericResponse UpdateCompany([FromBody] RequestUpdateCompany request)
    {
        var token = Request.Headers["Authorization"];
        var companyId = int.Parse(Request.RouteValues["companyId"].ToString());
        var response = _companyController.UpdateCompany(token, companyId, request.ValidationType);
        var genericResponse = new GenericResponse();
        genericResponse.Data = response;
        genericResponse.ExecutionSuccessful = true;
        genericResponse.Message = "Company updated successfully";
        return genericResponse;
    }
    
    [HttpPost("{companyId}/imports")]
    [AuthorizationFilter("CompanyOwner")]
    public GenericResponse ExecuteImport([FromBody] RequestImport request)
    {
        var token = Request.Headers["Authorization"];
        var companyId = int.Parse(Request.RouteValues["companyId"].ToString());
        var response = _importController.Import(request.ImporterName, request.SourcePath, companyId, token);
        var genericResponse = new GenericResponse();
        genericResponse.ExecutionSuccessful = true;
        genericResponse.Message = "Import executed successfully";
        genericResponse.Data = response;
        return genericResponse;
    }
    
    
}