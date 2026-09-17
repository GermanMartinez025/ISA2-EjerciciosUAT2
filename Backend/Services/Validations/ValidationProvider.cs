using System.Reflection;
using ModelException;
using ModeloValidador.Abstracciones;
using ServicesInterfaces.Validations;

namespace Services.Validations;

public class ValidationProvider : IValidationProvider
{
    private readonly Dictionary<string, Type> _validatorTypes;
    private const string PATH = "Assemblies/ModelValidation.dll";

    public ValidationProvider()
    {
        var assembly = Assembly.LoadFrom(PATH);

        if (assembly == null)
        {
            throw new BadRequestException("Could not load the specified assembly.");
        }

        _validatorTypes = assembly.GetTypes()
            .Where(t => typeof(IModeloValidador).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .ToDictionary(t => t.Name, t => t, StringComparer.OrdinalIgnoreCase);
    }

    public IModeloValidador GetValidator(string validatorName)
    {
        if (!_validatorTypes.ContainsKey(validatorName))
        {
            throw new NotFoundException($"No validator found with the name '{validatorName}'.");
        }

        return (IModeloValidador)Activator.CreateInstance(_validatorTypes[validatorName]);
    }
    
    public List<IModeloValidador> GetAllValidators()
    {
        return _validatorTypes.Values.Select(t => (IModeloValidador)Activator.CreateInstance(t)).ToList();
    }
}
