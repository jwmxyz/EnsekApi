using Ensek.Services.Models;

namespace Ensek.Services;

public interface IUploadServices<T>
{
    /// <summary>
    /// Used to upload a csv filestream
    /// </summary>
    /// <param name="fileStream">The incoming filestream to upload.</param>
    /// <returns>The csv parsing results object</returns>
    Task<CsvParsingResults<T>> Upload(Stream fileStream);
}