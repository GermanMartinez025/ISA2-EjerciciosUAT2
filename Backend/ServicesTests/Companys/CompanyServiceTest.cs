
using System.Reflection;
using IRepositories.Repositories.CompanyRepositories;
using ModelException;
using ModelInterface.Companys;
using ModelInterface.Users.UserType;
using Models.Company;
using Models.Users.UserTypes;
using ModelsAPI.Companies;
using ModelsAPI.Users;
using Moq;
using Services.Companys;
using ServicesInterfaces.Company;
using ServicesInterfaces.Users;

namespace ServicesTests.Companys;

[TestClass]
public class CompanyServiceTest
{
    private Mock<ICompanyRepository> _companyRepository;
    private ICompanyService _companyServices;
    private Mock<Company> _company;
    private Mock<ICompanyOwnerService> _companyOwnerService;
    private RequestAddCompanyToOwner _request;

    [TestInitialize]
    public void Setup()
    {
        _companyRepository = new Mock<ICompanyRepository>();
        _companyOwnerService = new Mock<ICompanyOwnerService>();
        _companyServices = new CompanyService(_companyRepository.Object, _companyOwnerService.Object);
        _company = new Mock<Company>();
    }

    [TestMethod]
    [ExpectedException(typeof(NotFoundException))]
    public void GetCompanyTest()
    {
        _companyRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(_company.Object);
        
        ACompany result = _companyServices.GetCompany(1);
        
        Assert.AreEqual(_company.Object, result);
    }

    [TestMethod]
    public void GetAllCompanysTest()
    {
        List<Company> companys = new List<Company>();

        _companyRepository.Setup(x => x.GetAll()).Returns(companys);
        List<ACompany> result = _companyServices.GetAllCompanys();

        Assert.IsTrue(companys.Cast<ACompany>().SequenceEqual(result));
    }

    [TestMethod]
    [ExpectedException(typeof(ConflictException))]
    public void GetCompany_NonExistingId_ReturnsNull()
    {
        _companyRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns((Company)null);

        ACompany result = _companyServices.GetCompany(-2);
    }

    [TestMethod]
    public void GetAllCompanys_EmptyList_ReturnsEmptyList()
    {
        List<Company> emptyList = new List<Company>();

        _companyRepository.Setup(x => x.GetAll()).Returns(emptyList);
        List<ACompany> result = _companyServices.GetAllCompanys();

        Assert.AreEqual(0, result.Count);
    }



    [TestMethod]
    public void AddCompanyToOwner_ValidOwner_AssignsCompanyCorrectly()
    {
        CompanyOwner companyOwner = new CompanyOwner();
        _companyRepository.Setup(x => x.Create(It.IsAny<Company>()));
        _companyOwnerService.Setup(x => x.GetById(It.IsAny<int>())).Returns(companyOwner);
        _companyOwnerService.Setup(x => x.AddCompanyToCompanyOwner(It.IsAny<CompanyOwner>(), It.IsAny<ACompany>()));

        var _request = new RequestAddCompanyToOwner
        {
            Rut = "212364450017", 
            Name = "pepe",        
            LogoType = "logoEmpresa",   
            ValidationType = "ModelValidatorBasic"
        };

        _companyServices.AddCompanyToOwner(1, _request);

        _companyRepository.Verify(repo => repo.Create(It.IsAny<Company>()), Times.Once);
        _companyOwnerService.Verify(x => x.AddCompanyToCompanyOwner(companyOwner, It.IsAny<ACompany>()), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(ConflictException))]
    public void AddCompanyToOwner_OwnerAlreadyHasCompanyAssigned()
    {
        CompanyOwner companyOwner = new CompanyOwner { Company = _company.Object };

        _companyOwnerService.Setup(x => x.GetById(It.IsAny<int>())).Returns(companyOwner);

        _companyServices.AddCompanyToOwner(1, _request);
    }

    [TestMethod]
    public void GetCompaniesTest()
    {
        var page = 1;
        var pageSize = 10;
        var filters = new Dictionary<string, object>();
        var companies = new List<Company>();

        _companyRepository.Setup(x => x.GetPaginated(page, pageSize, filters)).Returns(companies);
        var result = _companyServices.GetCompanies(page, pageSize, filters);

        Assert.IsTrue(companies.Cast<ACompany>().SequenceEqual(result));
    }

    [TestMethod]
    public void GetAmountOfCompanyTest()
    {
        var filters = new Dictionary<string, object>();
        var count = 10;

        _companyRepository.Setup(x => x.Count(filters)).Returns(count);

        var result = _companyServices.GetAmountOfCompanies(filters);

        Assert.AreEqual(count, result);
    }

    [TestMethod]
    public void GetCompanies_EmptyList_ReturnsEmptyList()
    {
        var page = 1;
        var pageSize = 10;
        var filters = new Dictionary<string, object>();
        var companies = new List<Company>();

        _companyRepository.Setup(x => x.GetPaginated(page, pageSize, filters)).Returns(companies);
        var result = _companyServices.GetCompanies(page, pageSize, filters);

        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void GetAmountOfCompany_EmptyList_ReturnsZero()
    {
        var filters = new Dictionary<string, object>();
        var count = 0;

        _companyRepository.Setup(x => x.Count(filters)).Returns(count);

        var result = _companyServices.GetAmountOfCompanies(filters);

        Assert.AreEqual(count, result);
    }

    [TestMethod]
    [ExpectedException(typeof(NotFoundException))]
    public void ValidateIdExisitsTest()
    {
        Company company = new Company();
        _companyRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(company);

        _companyServices.GetCompany(1);

        _companyRepository.Verify(repo => repo.GetById(It.IsAny<int>()), Times.Once);
    }
    
    [TestMethod]
    public void GetCompanies_ShouldReturnCompanies_WhenDataIsValid()
    {
        CompanyOwner companyOwner = new CompanyOwner("nombre", "apellido","prueba@mail.com", "12345678@");
        var companies = new List<Company>
        {
            new Company("212364450017", "Test Company 1", "Logo1", companyOwner, "ModelValidatorBasic"),
            new Company("212364450017", "Test Company 2", "Logo2", companyOwner, "ModelValidatorBasic")
        };
        _companyRepository
            .Setup(repo => repo.GetPaginated(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Dictionary<string, object>>()))
            .Returns(companies);
        
        var result = _companyServices.GetCompanies(1, 10, null);
        
        Assert.AreEqual(2, result.Count);
        Assert.IsInstanceOfType(result[0], typeof(ACompany));
        Assert.AreEqual("Test Company 1", result[0].Name);
    }
    
    [TestMethod]
    public void GetCompanies_ShouldHandleEmptyList()
    {
        _companyRepository
            .Setup(repo => repo.GetPaginated(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Dictionary<string, object>>()))
            .Returns(new List<Company>());
        
        var result = _companyServices.GetCompanies(1, 10, null);
        
        Assert.AreEqual(0, result.Count);
    }
    
    [TestMethod]
    public void GetCompanies_ShouldHandleSingleCompany()
    {
        CompanyOwner companyOwner = new CompanyOwner("nombre", "apellido","prueba@mail.com", "12345678@");
        var companies = new List<Company>
        {
            new Company("212364450017", "Test Company 1", "Logo1", companyOwner, "ModelValidatorBasic")
        };
        _companyRepository
            .Setup(repo => repo.GetPaginated(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Dictionary<string, object>>()))
            .Returns(companies);
        
        var result = _companyServices.GetCompanies(1, 10, null);
        
        Assert.AreEqual(1, result.Count);
        Assert.IsInstanceOfType(result[0], typeof(ACompany));
        Assert.AreEqual("Test Company 1", result[0].Name);
    }
    
    [TestMethod]
    public void ValidateIdIsGreaterThanZero_ValidId_DoesNotThrow()
    {
        int validId = 1;
        
        var validateIdMethod = typeof(CompanyService).GetMethod("ValidateIdIsGreaterThanZero", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        validateIdMethod.Invoke(_companyServices, new object[] { validId });
    }
    
    [TestMethod]
    public void ValidateIdIsGreaterThanZero_IdIsZero_ThrowsException()
    {
        int invalidId = 0;
        
        var validateIdMethod = typeof(CompanyService).GetMethod("ValidateIdIsGreaterThanZero",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        try
        {
            validateIdMethod.Invoke(_companyServices, new object[] { invalidId });
            Assert.Fail("Expected ArgumentOutOfRangeException was not thrown.");
        }
        catch (TargetInvocationException ex)
        {
            Assert.IsInstanceOfType(ex.InnerException, typeof(ConflictException));
        }
    }

    [TestMethod]
    public void ValidateIdIsGreaterThanZero_IdIsNegative_ThrowsException()
    {
        int invalidId = -1;
        
        var validateIdMethod = typeof(CompanyService).GetMethod("ValidateIdIsGreaterThanZero",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        try
        {
            validateIdMethod.Invoke(_companyServices, new object[] { invalidId });
            Assert.Fail("Expected ArgumentOutOfRangeException was not thrown.");
        }
        catch (TargetInvocationException ex)
        {
            Assert.IsInstanceOfType(ex.InnerException, typeof(ConflictException));
        }
    }
    
    [TestMethod]
    public void ValidateIdExits_ValidCompanyAndId_NoExceptionThrown()
    {
        int validId = 1;
        Company company = new Company { Id = validId };

        var validateIdExitsMethod = typeof(CompanyService).GetMethod("ValidateIdExits",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        validateIdExitsMethod.Invoke(_companyServices, new object[] { company, validId });
        
        Assert.IsTrue(true);
    }

    [TestMethod]
    public void ValidateIdExits_IdDoesNotMatchCompanyId_ThrowsException()
    {
        int invalidId = 2;
        Company company = new Company { Id = 1 };

        var validateIdExitsMethod = typeof(CompanyService).GetMethod("ValidateIdExits",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        try
        {
            validateIdExitsMethod.Invoke(_companyServices, new object[] { company, invalidId });
            Assert.Fail("Expected ArgumentException was not thrown.");
        }
        catch (TargetInvocationException ex)
        {
            Assert.IsInstanceOfType(ex.InnerException, typeof(NotFoundException));
            Assert.AreEqual("The provided ID does not match the company's ID.", ex.InnerException.Message);
        }
    }

    
    [TestMethod]
    public void GetMyCompanyTest()
    {
        var companyOwner = new Mock<ACompanyOwner>(); 
        companyOwner.Setup(x => x.CompanyId).Returns(1);
        companyOwner.Setup(x => x.HasCompanyAssigned).Returns(true);
        companyOwner.Setup(x => x.Company).Returns(_company.Object);
        
        _companyOwnerService.Setup(x => x.GetById(It.IsAny<int>())).Returns(companyOwner.Object);
        _companyRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(_company.Object);
        
        var result = _companyServices.GetMyCompany(1);
        
        Assert.AreEqual(_company.Object, result);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ConflictException))]
    public void GetMyCompany_NoCompanyAssigned()
    {
        var companyOwner = new Mock<ACompanyOwner>();
        companyOwner.Setup(x => x.HasCompanyAssigned).Returns(false);
        
        _companyOwnerService.Setup(x => x.GetById(It.IsAny<int>())).Returns(companyOwner.Object);
        
         _companyServices.GetMyCompany(1);
    }
    
    [TestMethod]
    public void UpdateCompanyTest()
    {
        var company = new Mock<Company>();
        company.Setup(x => x.Id).Returns(1);
        company.SetupProperty(x => x.ValidationType);
        company.Setup(x=> x.CompanyOwnerId).Returns(1);
            
        _companyRepository.Setup(x => x.GetById(1)).Returns(company.Object);
        _companyRepository.Setup(x => x.Update(It.IsAny<Company>()));
        
        

        var result = _companyServices.UpdateCompany(1, 1,"ModelValidatorBasic");

        Assert.AreEqual("ModelValidatorBasic", result.ValidationType);
    }
}
    



