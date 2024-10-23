using Microsoft.AspNetCore.Http;

public interface IFileValidationService<T> where T : Exception, new()
{
    void ValidateFile(IFormFile file);
}