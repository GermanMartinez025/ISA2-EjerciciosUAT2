using ModeloValidador.Abstracciones;

namespace ModelsAPI.ValidatorModels;

public class ResponseModelValidators
{
    public List<string> Validators { get; set; }
    
    
    public ResponseModelValidators(List<IModeloValidador> validators)
    {
        Validators = validators.Select(v => v.GetType().Name).ToList();
    }
    
}