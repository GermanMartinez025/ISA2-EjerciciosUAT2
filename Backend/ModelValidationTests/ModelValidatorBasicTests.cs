using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModeloValidador.Abstracciones;
using ModelValidation;


namespace ModelValidationInterfaces;

[TestClass]
public class ModelValidatorBasicTests
{

    private ModelValidatorBasic _validator;

    [TestInitialize]
    public void SetUp()
    {
        _validator = new ModelValidatorBasic();
    }

    [TestMethod]
    public void EsValido_ValidModel_ReturnsTrue()
    {
        var modelo = new Modelo { Value = "ABCDEF" };
        Assert.IsTrue(_validator.EsValido(modelo));
    }
    
    [TestMethod]
    public void EsValido_ValidModel_ReturnsFalse()
    {
        var modelo = new Modelo { Value = "ABCDE" };
        Assert.IsFalse(_validator.EsValido(modelo));
    }
    
    [TestMethod]
    public void EsValido_ValidModel_ReturnsFalse1()
    {
        var modelo = new Modelo { Value = "ABCDE1" };
        Assert.IsFalse(_validator.EsValido(modelo));
    }
    
    [TestMethod]
    public void EsValido_ValidModel_ReturnsFalse2()
    {
        var modelo = new Modelo { Value = "1BCDEF" };
        Assert.IsFalse(_validator.EsValido(modelo));
    }
    
    [TestMethod]
    public void EsValido_ValidModel_ReturnsFalse3()
    {
        var modelo = new Modelo { Value = "A1CDEF" };
        Assert.IsFalse(_validator.EsValido(modelo));
    }
    
    [TestMethod]
    public void EsValido_ValidModel_ReturnsFalse4()
    {
        var modelo = new Modelo { Value = "AB1DeF" };
        Assert.IsFalse(_validator.EsValido(modelo));
    }
    
    [TestMethod]
    public void EsValido_ValidModel_ReturnsFalse5()
    {
        var modelo = new Modelo { Value = "ABC1EF" };
        Assert.IsFalse(_validator.EsValido(modelo));
    }
    
    [TestMethod]
    public void EsValido_ValidModel_ReturnsTrue2()
    {
        var modelo = new Modelo { Value = "abcdef" };
        Assert.IsTrue(_validator.EsValido(modelo));
    }
    
    

}