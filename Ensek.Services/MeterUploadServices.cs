using Ensek.DataAccess.DbModels;
using Ensek.Services.Models;
using Ensek.Services.Models.CsvMappings;

namespace Ensek.Services;

public class MeterUploadServices : IUploadServices<MeterReadingRecord>
{
    private readonly ICsvServices _csvServices;

    public MeterUploadServices(ICsvServices csvServices)
    {
        _csvServices = csvServices;
    }

    /// <inheritdoc>
    ///     <cref>IUploadsServices.Upload(Stream)</cref>
    /// </inheritdoc>
    public async Task<CsvParsingResults<MeterReadingRecord>> Upload(Stream fileStream)
    {
        try
        {
            var records = _csvServices.Read<MeterReadingRecord, MeterReadingMapping>(fileStream);
            var deduplicatedRecords = _csvServices.DeduplicateCallReferences(records);
            //_logger.LogInformation($"Processed {records.Count} records, deduplicated to {deduplicatedRecords.ValidRecords.Count} valid records.");
            //await _callRecordRepository.SaveCallRecordsAsync(deduplicatedRecords.ValidRecords);

            return deduplicatedRecords;
        }
        catch (Exception ex)
        {
            // Log the exception for debugging purposes
            //_logger.LogError(ex, "Error occurred while uploading CSV file.");
            throw; // Re-throw the exception for further handling if needed
        }
    }
}