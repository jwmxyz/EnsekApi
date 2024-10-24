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

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!ValidCsvExtensions.Contains(extension))
        {
            throw Activator.CreateInstance(typeof(T), "Invalid file extension: {Extension}") as T;
        }

        if (!ValidCsvMimeTypes.Contains(file.ContentType.ToLowerInvariant()))
        {
            throw Activator.CreateInstance(typeof(T), $"Invalid MIME type: {file.ContentType.ToLowerInvariant()}") as T;
        }

        //todo this is specific to csvfiles and should not be part of the generic method.
        try
        {
            using var reader = new StreamReader(file.OpenReadStream());
            var firstLine = reader.ReadLine();
            if (string.IsNullOrWhiteSpace(firstLine))
            {
                throw Activator.CreateInstance(typeof(T), "Empty first line in CSV file") as T;
            }

            const string csvPattern = @"^[^,;|""\t\r\n]*(?:,[^,;|""\t\r\n]*)*$";
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