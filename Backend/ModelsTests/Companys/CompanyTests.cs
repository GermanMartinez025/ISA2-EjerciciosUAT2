using ModelException;
using ModelInterface.Users;
using Models.Company;
using Models.Users.UserTypes;
using Moq;

namespace ModelsTests.Companys;

[TestClass]
public class CompanyTests
{
    private Company _company;
    private int _id;
    private string _rut;
    private string _name;
    private string _logoType;
    private CompanyOwner _companyOwner;
    private string _validationType;

    [TestInitialize]
    public void TestInitialize()
    {
        _id = 1;
        _rut = "212364450017";
        _name = "El exito";
        _logoType = "www.prueba.com.uy ";
        _companyOwner = new Mock<CompanyOwner>().Object;
        _validationType = "Basic";
    }

    [TestMethod]
    public void NoParametrizedCompanyConstructor_WhenCalled_CreatesNewCompany()
    {
        _company = new Company();

        Assert.IsNotNull(_company);
    }

    [TestMethod]
    public void ParametrizedCompanyContructor_WhenCalled_CreateNewCompany()
    {
        _company = new Company(_rut, _name, _logoType, _companyOwner, _validationType) { Id = _id };
        Assert.AreEqual(_company.Id, _id);
        Assert.AreEqual(_company.Rut, _rut);
        Assert.AreEqual(_company.LogoType, _logoType);
        Assert.AreEqual(_company.CompanyOwner, _companyOwner);
        Assert.AreEqual(_company.CompanyOwnerId, _companyOwner.UserId);
        Assert.IsNotNull(_company.Devices);
    }

    [TestMethod]
    public void ParametrizedCompanyConstructor_WhenCompanyOwnerIsNull_ThrowsException()
    {
        _companyOwner = null;
        

        Assert.ThrowsException<NotFoundException>(() => new Company(_rut, _name, _logoType, _companyOwner, _validationType));

    }

    [TestMethod]
    public void ValideRutHave12Numbers_ThrowsException()
    {
        _rut = "21236445001";
        

        Assert.ThrowsException<BadRequestException>(() => new Company(_rut, _name, _logoType, _companyOwner, _validationType));
    }
    
    [TestMethod]
    public void ValidateWhenFirstTwoDigitsInRutAreLessThanOne()
    {
        _rut = "002345678901";

        Assert.ThrowsException<BadRequestException>(() => new Company(_rut, _name, _logoType, _companyOwner, _validationType));

    }
    
    [TestMethod]
    public void ValidateWhenFirstTwoDigitsInRutAreBiggerThanTwentyOne()
    {
        _rut = "222345678901";


        Assert.ThrowsException<BadRequestException>(() => new Company(_rut, _name, _logoType, _companyOwner, _validationType));

    }
    
    [TestMethod]
    public void ValidatePositionNineAndTenOfRut()
    {
        _rut = "212345670101";
        

        Assert.ThrowsException<BadRequestException>(() => new Company(_rut, _name, _logoType, _companyOwner, _validationType));

    }
    
    [TestMethod]
    public void ValidatePositionThreeToEightOfRut()
    {
        _rut = "120000000045";

        Assert.ThrowsException<BadRequestException>(() => new Company(_rut, _name, _logoType, _companyOwner, _validationType));

    }
    
    [TestMethod]
    public void ValidateWhenNameIsNull()
    {
        _name = null;

        Assert.ThrowsException<NotFoundException>(() => new Company(_rut, _name, _logoType, _companyOwner, _validationType));

    }
    
    [TestMethod]
    public void ValidateWhenNameIsEmpty()
    {
        _name = "";

        Assert.ThrowsException<NotFoundException>(() => new Company(_rut, _name, _logoType, _companyOwner, _validationType));

    }
    
    [TestMethod]
    public void ValidateWhenLogoTypeIsNull()
    {
        _logoType = null;

        Assert.ThrowsException<NotFoundException>(() => new Company(_rut, _name, _logoType, _companyOwner, _validationType));

    }
    
    [TestMethod]
    public void ValidateWhenLogoTypeIsEmpty()
    {
        _logoType = "";

        Assert.ThrowsException<NotFoundException>(() => new Company(_rut, _name, _logoType, _companyOwner, _validationType));

    }

    [TestMethod]
    public void Constructor_WhenCalled_SetValidationType()
    {
        var company = new Company(_rut, _name, _logoType, _companyOwner, _validationType);
        Assert.AreEqual(company.ValidationType, _validationType);
    }
    
    [TestMethod]
    public void Constructor_WhenValidationTypeIsEmpty_ThrowsException()
    {
        _validationType = "";
        Assert.ThrowsException<BadRequestException>(() => new Company(_rut, _name, _logoType, _companyOwner, _validationType));
    }
    
}