using Microsoft.AspNetCore.Http;

public interface IFileValidationService<T> where T : Exception, new()
{
    /// <summary>
    /// Method used to determine whether or not a file is valid.
    ///
    /// todo This should probably return something and not throw exceptions when invalid.
    /// todo return (bool isValid, Outcome outcome) ???
    /// </summary>
    /// <param name="file">The file we want to validate.</param>
    void ValidateFile(IFormFile file);
}