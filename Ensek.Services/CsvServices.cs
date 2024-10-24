using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Ensek.Services.Models;

namespace Ensek.Services;

public class CsvServices : ICsvServices
{

    /// <inheritdoc cref="ICsvServices.Read{T, TK}(Stream, bool)" />
    public CsvParsingResults<T> Read<T, TK>(Stream file, bool firstRowIsHeading = true) where TK : ClassMap<T>
    {
        var reader = new StreamReader(file);
        var errors = new List<string>();
        var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = firstRowIsHeading,
            ReadingExceptionOccurred = e =>
            {
                errors.Add(
                    $"Error at row {e.Exception.Context?.Parser?.Row} : {e.Exception.Context?.Parser?.RawRecord}");
                return false;
            }
        };
        var csv = new CsvReader(reader, configuration);
        csv.Context.RegisterClassMap<TK>();
        var records = csv.GetRecords<T>().ToList();
        return new CsvParsingResults<T>
        {
            InvalidRecords = errors,
            ValidRecords = records,
        };
    }

    /// <inheritdoc cref="ICsvServices.Deduplicate{T}(CsvParsingResults{T}, Func{T, object})" />
    public CsvParsingResults<T> Deduplicate<T>(CsvParsingResults<T> results, Func<T, object> keySelector) where T : new()
    {
        var groupedRecords = results.ValidRecords
            .GroupBy(keySelector);

        foreach (var group in groupedRecords.Where(x => x.Count() > 1))
        {
            var records = group.ToList();
            foreach (var duplicate in records.Skip(1))
            {
                results.InvalidRecords.Add(duplicate.ToString());
            }
        }

        results.ValidRecords = groupedRecords
            .Select(g => g.First())  
            .ToList();

        return results;
    }
}