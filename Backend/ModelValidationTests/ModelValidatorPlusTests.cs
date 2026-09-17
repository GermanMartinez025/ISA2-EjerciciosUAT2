using ModeloValidador.Abstracciones;
using ModelValidation;

namespace ModelValidationInterfaces;

[TestClass]
public class ModelValidatorPlusTests
{
    
    private ModelValidatorPlus _validator;

    [TestInitialize]
    public void SetUp()
    {
        _validator = new ModelValidatorPlus();
    }
    
    [TestMethod]
    public void EsValido_ValidModel_ReturnsTrue()
    {
        var modelo = new Modelo { Value = "ABC123" };
        Assert.IsTrue(_validator.EsValido(modelo));
    }
    
    [TestMethod]
    public void EsValido_ValidModel_ReturnsTrue1()
    {
        var modelo = new Modelo { Value = "abc123" };
        Assert.IsTrue(_validator.EsValido(modelo));
    }
    
    [TestMethod]
    public void EsValido_ValidModel_ReturnsFalse()
    {
        var modelo = new Modelo { Value = "AB1234" };
        Assert.IsFalse(_validator.EsValido(modelo));
    }
    
    [TestMethod]
    public void EsValido_ValidModel_ReturnsFalse1()
    {
        var modelo = new Modelo { Value = "1BC123" };
        Assert.IsFalse(_validator.EsValido(modelo));
    }
    
    [TestMethod]
    public void EsValido_ValidModel_ReturnsFalse2()
    {
        var modelo = new Modelo { Value = "A2C123" };
        Assert.IsFalse(_validator.EsValido(modelo));
    }
    
    [TestMethod]
    public void EsValido_ValidModel_ReturnsFalse3()
    {
        var modelo = new Modelo { Value = "AB1123" };
        Assert.IsFalse(_validator.EsValido(modelo));
    }
    
    [TestMethod]
    public void EsValido_ValidModel_ReturnsFalse4()
    {
        var modelo = new Modelo { Value = "ABCA23" };
        Assert.IsFalse(_validator.EsValido(modelo));
    }
    
    [TestMethod]
    public void EsValido_ValidModel_ReturnsFalse5()
    {
        var modelo = new Modelo { Value = "ABC1B3" };
        Assert.IsFalse(_validator.EsValido(modelo));
    }
    
    [TestMethod]
    public void EsValido_ValidModel_ReturnsFalse6()
    {
        var modelo = new Modelo { Value = "ABC12C" };
        Assert.IsFalse(_validator.EsValido(modelo));
    }

}