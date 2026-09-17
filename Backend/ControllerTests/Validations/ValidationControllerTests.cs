using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServicesInterfaces.Validations;
using ModeloValidador.Abstracciones;
using Controllers.Validations;
using ModelsAPI.ValidatorModels;
using System.Collections.Generic;

[TestClass]
public class ValidationControllerTests
{
    private Mock<IValidationProvider> _mockValidationProvider;
    private ValidationController _controller;

    [TestInitialize]
    public void TestInitialize()
    {
        _mockValidationProvider = new Mock<IValidationProvider>();
        _controller = new ValidationController(_mockValidationProvider.Object);
    }

    [TestMethod]
    public void GetAllValidators_WhenCalled_ReturnsResponseModelValidators()
    {
        var mockValidators = new List<IModeloValidador>
        {
            new Mock<IModeloValidador>().Object,
            new Mock<IModeloValidador>().Object
        };

        _mockValidationProvider.Setup(v => v.GetAllValidators()).Returns(mockValidators);

        var result = _controller.GetAllValidators();

        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(ResponseModelValidators));
        Assert.AreEqual(mockValidators.Count, result.Validators.Count);
    }

    [TestMethod]
    public void GetAllValidators_WhenNoValidators_ReturnsEmptyResponse()
    {
        _mockValidationProvider.Setup(v => v.GetAllValidators()).Returns(new List<IModeloValidador>());

        var result = _controller.GetAllValidators();

        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(ResponseModelValidators));
        Assert.AreEqual(0, result.Validators.Count);
    }
}