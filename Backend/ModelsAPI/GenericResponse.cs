namespace ModelsAPI;

public class GenericResponse
{
    public bool ExecutionSuccessful { get; set; }
    public string Message { get; set; }
    public object? Data { get; set; }
}