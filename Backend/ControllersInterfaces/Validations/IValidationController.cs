using ModelsAPI.ValidatorModels;

namespace ControllersInterfaces.Validations;

public interface IValidationController
{
    ResponseModelValidators GetAllValidators();
}