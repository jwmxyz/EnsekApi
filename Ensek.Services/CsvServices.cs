using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Ensek.DataAccess.DbModels;
using Ensek.Services.Models;

namespace Ensek.Services;

public class CsvServices : ICsvServices
{
    // private readonly ICdrErrorManager _cdrErrorManager;

    public CsvServices()
    {
        //_cdrErrorManager = cdrErrorManager;
    }

    /// <inheritdoc cref="ICsvServices.Read{T, TK}(Stream, bool)" />
    public CsvParsingResults<T> Read<T, TK>(Stream file, bool firstRowIsHeading = true) where TK : ClassMap<T>
    {
        try
        {
            var reader = new StreamReader(file);
            var errors = new List<string>();
            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = firstRowIsHeading,
                ReadingExceptionOccurred = e =>
                {
                    errors.Add($"Error at row {e.Exception.Context?.Parser?.Row} : {e.Exception.Context?.Parser?.RawRecord}");
                    //errors.Add($"Error at row {e.Exception.Context?.Parser?.RawRecord}: {e.Exception.Message}");
                    //_cdrErrorManager.LogErrorAndReturnException<Exception>(e.Exception.Message, e.Exception);
                    return false;
                }
            };
            var csv = new CsvReader(reader, configuration);
            csv.Context.RegisterClassMap<TK>();
            var records = csv.GetRecords<T>().ToList();
            return new CsvParsingResults<T>
            {
                InvalidRecords = errors,
                ValidRecords = records
            };
        }
        catch (Exception ex)
        {
            throw;
            //throw _cdrErrorManager.LogErrorAndReturnException<InvalidCsvException>("Invalid CSV - Error When parsing", ex.InnerException);
        }
    }

    /// <inheritdoc cref="ICsvServices.DeduplicateCallReferences(CsvParsingResults{MeterReadingRecord})" />
    public CsvParsingResults<MeterReadingRecord> DeduplicateCallReferences(CsvParsingResults<MeterReadingRecord> results)
    {
        var duplicateItems = results.ValidRecords.GroupBy(x => new {x.AccountId, x.Value, x.DateTime}).Where(x => x.Count() > 1);
        foreach (var item in duplicateItems)
        {
            results.DuplicateRecords.Add($"{item.Key.AccountId}, {item.Key.DateTime.ToString("dd/MM/yyyy HH:mm")}, {item.Key.Value})");
        }

        results.ValidRecords = results
            .ValidRecords.GroupBy(x => new {x.AccountId, x.Value, x.DateTime})
            .Where(x => x.Count() == 1)
            .SelectMany(x => x.ToList()).ToList();
        return results;
    }
}