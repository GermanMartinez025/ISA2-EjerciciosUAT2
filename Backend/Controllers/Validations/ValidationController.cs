using ControllersInterfaces.Validations;
using ModelsAPI.ValidatorModels;
using ServicesInterfaces.Validations;

namespace Controllers.Validations;

public class ValidationController : IValidationController
{
    private readonly IValidationProvider _validationProvider;

    public ValidationController(IValidationProvider validationProvider)
    {
        _validationProvider = validationProvider;
    }

    public ResponseModelValidators GetAllValidators()
    {
        var validators = _validationProvider.GetAllValidators();

        return new ResponseModelValidators(validators);
    }
    
}