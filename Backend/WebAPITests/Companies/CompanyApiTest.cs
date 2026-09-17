using ControllersInterfaces.Companies;
using ControllersInterfaces.Importers;
using ControllersInterfaces.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelInterface.Companys;
using Models.Importers;
using ModelsAPI;
using ModelsAPI.Companies;
using ModelsAPI.Importers;
using Moq;
using WebAPI.APIs;

namespace WebAPITests.Companies;

[TestClass]
public class CompanyApiTest
{
    private Mock<IUserController> _userController;
    private Mock<ICompanyController> _mockCompanyController;
    private Mock<IImporterController> _importerController;
    private CompanyAPI _companyApi;
    
    
    [TestInitialize]
    public void Initialize()
    {
        _mockCompanyController = new Mock<ICompanyController>();
        _importerController = new Mock<IImporterController>();
        _companyApi = new CompanyAPI(_mockCompanyController.Object, _importerController.Object);
        HttpContext httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["Authorization"] = "token";
        httpContext.Request.RouteValues = new Microsoft.AspNetCore.Routing.RouteValueDictionary();
        httpContext.Request.RouteValues["companyId"] = "1";
        var controllerContext = new ControllerContext { HttpContext = httpContext };
        _companyApi = new CompanyAPI(_mockCompanyController.Object, _importerController.Object);
        _companyApi.ControllerContext = controllerContext;
    }

    [TestMethod]
    public void AddCompany_ShouldReturnOk_WhenCompanyAddedSuccessfully()
    {
        RequestAddCompanyToOwner request = new RequestAddCompanyToOwner() { Rut = "212364450017",Name = "Companys Name", LogoType = "www.laImagen.com/aqui" };
        ResponseAddCompanyToOwner response = new ResponseAddCompanyToOwner();

        _mockCompanyController.Setup(c => c.AddOneCompanyToOwner("token", request)).Returns(response);
        
        var result = _companyApi.AddOneCompanyToOwner(request);

        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(GenericResponse));
    }
    
    [TestMethod]
    public void GetCompanies_ShouldReturnOk_WhenCompaniesAreRetrieved()
    {
        int page = 1;
        int pageSize = 10;
        List<string> companyNames = new List<string>();
        List<string> ownerFullNames = new List<string>();
        ResponseGetComapanies response = new ResponseGetComapanies();

        _mockCompanyController.Setup(c => c.GetCompanies(page, pageSize, companyNames, ownerFullNames)).Returns(response);

        var result = _companyApi.GetCompanies(page, pageSize, companyNames, ownerFullNames);

        Assert.IsNotNull(result);
    }
    
    [TestMethod]
    public void GetMyCompany_ShouldReturnOk_WhenCompanyIsRetrieved()
    {
        ResponseCompany response = new ResponseCompany();

        _mockCompanyController.Setup(c => c.GetMyCompany("token")).Returns(response);

        var result = _companyApi.GetMyCompany();

        Assert.IsNotNull(result);
    }
    
    [TestMethod]
    public void UpdateCompany_ShouldReturnOk_WhenCompanyIsUpdated()
    {
        var companyId = 1;
        var modelValidator = "ModelValidatorBasic";
        var company = new Mock<ACompany>();
        company.Setup(x => x.Id).Returns(1);
        
        RequestUpdateCompany request = new RequestUpdateCompany() { ValidationType = "ModelValidatorBasic" };
        ResponseCompany response = new ResponseCompany();

        _mockCompanyController.Setup(c => c.UpdateCompany("token", companyId, modelValidator)).Returns(response);

        var result = _companyApi.UpdateCompany(request);

        Assert.IsNotNull(result);
        Assert.AreEqual(response, result.Data);
    }
    
    [TestMethod]
    public void ExecuteImport_ShouldCallControllerMethod()
    {
        var companyId = 1;
        var token = "token";
        
        var request = new RequestImport("JsonImporter", "path/to/devices.json");
        _importerController.Setup(c => c.Import(request.ImporterName, request.SourcePath, companyId, token)).Returns(new List<ImportedDevice>());
        
        var result = _companyApi.ExecuteImport(request);

        _importerController.Verify(c => c.Import(request.ImporterName, request.SourcePath, companyId, token), Times.Once);
    }
}