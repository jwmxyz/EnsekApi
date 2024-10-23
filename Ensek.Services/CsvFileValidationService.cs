using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

public class CsvFileValidationService<T> : IFileValidationService<T> where T : Exception, new()
{
    
    //todo move these to appsettings?
    private static readonly string[] ValidCsvMimeTypes =
    [
        "text/csv",
        "application/csv",
        "text/x-csv",
        "application/x-csv",
        "text/comma-separated-values",
        "text/x-comma-separated-values"
    ];

    private static readonly string[] ValidCsvExtensions =
    [
        ".csv"
    ];

    public void ValidateFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw Activator.CreateInstance(typeof(T), "Attempted to validate null or empty file") as T;
        }

        // Check file extension
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!ValidCsvExtensions.Contains(extension))
        {
            throw Activator.CreateInstance(typeof(T), "Invalid file extension: {Extension}") as T;
        }

        // Check MIME type
        if (!ValidCsvMimeTypes.Contains(file.ContentType.ToLowerInvariant()))
        {
            throw Activator.CreateInstance(typeof(T), $"Invalid MIME type: {file.ContentType.ToLowerInvariant()}") as T;
        }

        try
        {
            using var reader = new StreamReader(file.OpenReadStream());
            // Read first line to check CSV structure
            var firstLine = reader.ReadLine();
            if (string.IsNullOrWhiteSpace(firstLine))
            {
                throw Activator.CreateInstance(typeof(T), "Empty first line in CSV file") as T;
            }

            // Basic CSV structure validation
            var csvPattern = @"^[^,;|""\t\r\n]*(?:,[^,;|""\t\r\n]*)*$";
            var isValid = Regex.IsMatch(firstLine, csvPattern);
            if (!isValid)
            {
                throw Activator.CreateInstance(typeof(T), "Invalid CSV structure in first line") as T;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}