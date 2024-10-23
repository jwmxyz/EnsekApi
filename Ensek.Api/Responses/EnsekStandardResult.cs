namespace Ensek.Api.Responses;

/// <summary>
/// An object that standardises the response objects from the API.
/// </summary>
public class EnsekStandardResult
{
    public object? Data { get; set; }
    public bool HasErrors { get; set; }
    public IEnumerable<string> Errors { get; set; }

    public EnsekStandardResult(object? response)
    {
        Data = response;
    }

    public EnsekStandardResult(object? response, bool isError, IEnumerable<string> errors) : this(response)
    {
        HasErrors = isError;
        Errors = errors;
    }

    public EnsekStandardResult(object? response, bool isError, string error) : this(response, isError,
        new List<string> { error })
    {
    }
}