using System.Text.RegularExpressions;
using ModeloValidador.Abstracciones;

namespace ModelValidation;

public class ModelValidatorPlus : IModeloValidador
{
    private readonly Regex _regex = new Regex(@"^[A-Za-z]{3}[0-9]{3}$");

    public bool EsValido(Modelo modelo)
    {
        return !string.IsNullOrWhiteSpace(modelo.Value) && _regex.IsMatch(modelo.Value);
    }
}