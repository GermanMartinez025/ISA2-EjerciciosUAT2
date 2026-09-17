using ModeloValidador.Abstracciones;

namespace ServicesInterfaces.Validations;

public interface IValidationProvider
{
    IModeloValidador GetValidator(string validatorName);
    List<IModeloValidador> GetAllValidators();
}