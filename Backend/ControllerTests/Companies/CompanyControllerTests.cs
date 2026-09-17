using Controllers.Companies;
using ModelException;
using ModelInterface.Companys;
using Models.Company;
using Models.Users;
using Models.Users.UserTypes;
using ModelsAPI.Companies;
using Moq;
using ServicesInterfaces.Company;
using ServicesInterfaces.Sessions;

namespace ControllerTests.Companies;

[TestClass]
public class CompanyControllerTests
{
    private Mock<ICompanyService> _companyService;
    private Mock<ISessionService> _sessionService;
    private CompanyController _companyController;
    private Mock<CompanyOwner> _companyOwner;
    
    [TestInitialize]
    public void Initialize()
    {
        _companyService = new Mock<ICompanyService>();
        _sessionService = new Mock<ISessionService>();
        _companyController = new CompanyController(_companyService.Object, _sessionService.Object);
        _companyOwner = new Mock<CompanyOwner>();
        _companyOwner.Setup(m => m.UserId).Returns(1);
        _companyOwner.Setup(m => m.User).Returns(new User());

    }

    [TestMethod]
    public void GetCompanies_ShouldReturnCompanies()
    {
        var companies = new List<ACompany>()
        {
            new Company("113826230014", "Company1", "logo1.png", _companyOwner.Object, "Basic"),
        };
        
        _companyService.Setup(m => m.GetCompanies(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Dictionary<string, object>>()))
            .Returns(companies);
        
        _companyService.Setup(m => m.GetAmountOfCompanies(It.IsAny<Dictionary<string, object>>())).Returns(1);
        
        var response = _companyController.GetCompanies(1, 10, null, null);
        
        Assert.AreEqual(1, response.TotalCompanies);
        Assert.AreEqual(1, response.Companies.Count);
    }
    
    
    [TestMethod]
    public void GetCompanies_WhenCompanyNameFilter_ShouldReturnCompanies()
    {
        var companies = new List<ACompany>()
        {
            new Company("113826230014", "Company1", "logo1.png", _companyOwner.Object, "Basic"),
        };
        
        _companyService.Setup(m => m.GetCompanies(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Dictionary<string, object>>()))
            .Returns(companies);
        
        _companyService.Setup(m => m.GetAmountOfCompanies(It.IsAny<Dictionary<string, object>>())).Returns(1);
        
        var response = _companyController.GetCompanies(1, 10, new List<string> { "Company1" }, null);
        
        Assert.AreEqual(1, response.TotalCompanies);
        Assert.AreEqual(1, response.Companies.Count);
    }
    
    [TestMethod]
    public void GetCompanies_WhenOwnerFullNameFilter_ShouldReturnCompanies()
    {
        var companies = new List<ACompany>()
        {
            new Company("113826230014", "Company1", "logo1.png", _companyOwner.Object,"Basic"),
        };
        
        _companyService.Setup(m => m.GetCompanies(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Dictionary<string, object>>()))
            .Returns(companies);
        
        _companyService.Setup(m => m.GetAmountOfCompanies(It.IsAny<Dictionary<string, object>>())).Returns(1);
        
        var response = _companyController.GetCompanies(1, 10, null, new List<string> { "Owner1" });
        
        Assert.AreEqual(1, response.TotalCompanies);
        Assert.AreEqual(1, response.Companies.Count);
    }
    
    [TestMethod]
    public void AddCompanyWithYourOwnerTest()
    {
        var request = new RequestAddCompanyToOwner()
        {
            Rut = "113826230014",
            Name = "Company1",
            LogoType = "logo1.png"
        };
        
        _sessionService.Setup(m => m.GetUserIdFromSession(It.IsAny<string>())).Returns(1);
        
        var company = new Company("113826230014", "Company1", "logo1.png", _companyOwner.Object, "Basic");
        
        _companyService.Setup(m => m.AddCompanyToOwner(It.IsAny<int>(), It.IsAny<RequestAddCompanyToOwner>()))
            .Returns(company);
        
        var response = _companyController.AddOneCompanyToOwner("token", request);
        
        Assert.AreEqual(company.Rut, response.Rut);
        Assert.AreEqual(company.Name, response.Name);
        Assert.AreEqual(company.LogoType, response.LogoType);
    }
    
    [TestMethod]
    public void GetMyCompanyTest()
    {
        _sessionService.Setup(m => m.GetUserIdFromSession(It.IsAny<string>())).Returns(1);
        
        var company = new Company("113826230014", "Company1", "logo1.png", _companyOwner.Object, "ModelValidatorBasic");
        
        _companyService.Setup(m => m.GetMyCompany(It.IsAny<int>())).Returns(company);
        
        var response = _companyController.GetMyCompany("token");
        
        Assert.AreEqual(company.Rut, response.CompanyRut);
        Assert.AreEqual(company.Name, response.CompanyName);
        Assert.AreEqual(company.LogoType, response.CompanyLogoType);
    }
    
    [TestMethod]
    public void UpdateCompanyTest()
    {
        _sessionService.Setup(m => m.GetUserIdFromSession(It.IsAny<string>())).Returns(1);
        
        var company = new Company("113826230014", "Company1", "logo1.png", _companyOwner.Object, "ModelValidatorBasic");
        
        _companyService.Setup(m => m.UpdateCompany(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
            .Returns(company);
        
        var response = _companyController.UpdateCompany("token", 1, "ModelValidatorBasic");
        
        Assert.AreEqual(company.Rut, response.CompanyRut);
        Assert.AreEqual(company.Name, response.CompanyName);
        Assert.AreEqual(company.LogoType, response.CompanyLogoType);
    }
    
    [TestMethod]
    public void UpdateCompany_WhenCompanyOwnerIsNotTheSame_ShouldThrowException()
    {
        _sessionService.Setup(m => m.GetUserIdFromSession(It.IsAny<string>())).Returns(1);
        
        var company = new Company("113826230014", "Company1", "logo1.png", _companyOwner.Object, "ModelValidatorBasic");
        
        _companyService.Setup(m => m.UpdateCompany(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
            .Throws(new ConflictException("Company owner is not the same"));
        
        Assert.ThrowsException<ConflictException>(() => _companyController.UpdateCompany("token", 1, "ModelValidatorBasic"));
    }
}