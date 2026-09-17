using ModelException;
using ModelValidation;
using Services.Validations;

namespace ServicesTests.Validations;

[TestClass]
public class ValidationProviderTests
{
    
    private ValidationProvider _provider;

    [TestInitialize]
    public void SetUp()
    {
        _provider = new ValidationProvider();
    }

    [TestMethod]
    public void GetValidator_ValidValidatorName_ReturnsValidatorInstance()
    {
        var validator = _provider.GetValidator("ModelValidatorBasic");

        Assert.IsNotNull(validator, "Validator should not be null.");
        Assert.IsInstanceOfType(validator, typeof(ModelValidatorBasic), "Validator should be of type ModelValidatorBasic.");
    }
    
    [TestMethod]
    public void GetValidator_InvalidValidatorName_ThrowsArgumentException()
    {
        Assert.ThrowsException<NotFoundException>(() => _provider.GetValidator("InvalidValidatorName"));
    }
    
    [TestMethod]
    public void GetAllValidators_ReturnsAllValidators()
    {
        var validators = _provider.GetAllValidators();

        Assert.AreEqual(2, validators.Count);
        Assert.IsInstanceOfType(validators[0], typeof(ModelValidatorBasic));
        Assert.IsInstanceOfType(validators[1], typeof(ModelValidatorPlus));
        Assert.AreEqual("ModelValidatorBasic", validators[0].GetType().Name);
        Assert.AreEqual("ModelValidatorPlus", validators[1].GetType().Name);
    }
    
}