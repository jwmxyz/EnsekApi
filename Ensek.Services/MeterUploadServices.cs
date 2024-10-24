using Ensek.DataAccess.DbModels;
using Ensek.DataAccess.Repositories;
using Ensek.Services.Models;
using Ensek.Services.Models.CsvMappings;
using Ensek.Services.Models.DTO;

namespace Ensek.Services;

public class MeterUploadServices(
    ICsvServices csvServices,
    IRepository<MeterReading> meterReadingRepository,
    IRepository<Account> accountRepository)
    : IUploadServices<MeterReadingRecord>
{
    /// <inheritdoc>
    ///     <cref>IUploadsServices.Upload(Stream)</cref>
    /// </inheritdoc>
    public async Task<CsvParsingResults<MeterReadingRecord>> Upload(Stream fileStream)
    {
        var records = csvServices.Read<MeterReadingRecord, MeterReadingMapping>(fileStream);
        var deduplicatedRecords =
            csvServices.Deduplicate(records, x => new { x.AccountId, x.ReadingValue, x.ReadingDateTime });

        await RemoveInvalidAccounts(deduplicatedRecords);
        await RemoveDuplicateExistingEntries(deduplicatedRecords);

        await meterReadingRepository.Insert(deduplicatedRecords.ValidRecords.Select(x => new MeterReading
        {
            AccountId = x.AccountId,
            ReadingValue = x.ReadingValue,
            ReadingDateTime = x.ReadingDateTime
        }));

        return deduplicatedRecords;
    }

    private async Task RemoveDuplicateExistingEntries(CsvParsingResults<MeterReadingRecord> records)
    {
        var inBoundValidReadings = new HashSet<MeterReadingRecord>(records.ValidRecords);

        //this should never be done in code. This should be a cache or via SQL comparing 2 db tables
        var validReadings = await meterReadingRepository.GetAll();

        var repositoryReadings = new HashSet<MeterReadingRecord>(
            validReadings.Select(reading => new MeterReadingRecord
            {
                AccountId = reading.AccountId,
                ReadingDateTime = reading.ReadingDateTime,
                ReadingValue = reading.ReadingValue
            })
        );
        
        var commonReadings = new HashSet<MeterReadingRecord>(inBoundValidReadings.Intersect(repositoryReadings, new MeterReadingRecordComparer()));

        foreach (var duplicate in commonReadings)
        {
            records.InvalidRecords.Add($"{duplicate} - entry already exists");
            records.ValidRecords.Remove(duplicate);
        }
    }

    private async Task RemoveInvalidAccounts(CsvParsingResults<MeterReadingRecord> records)
    {
        var inBoundValidAccountIds = new HashSet<int>(records.ValidRecords.Select(record => record.AccountId));

        var validAccounts = await accountRepository.GetAll(
            account => inBoundValidAccountIds.Contains(account.Id)
        );

        var validAccountsIds = new HashSet<int>(validAccounts.Select(account => account.Id));

        var missingAccountIds = inBoundValidAccountIds.Except(validAccountsIds).ToList();
        
        var recordsToRemove = records.ValidRecords
            .Where(record => missingAccountIds.Contains(record.AccountId))
            .ToList(); // Gather all records to be removed

        foreach (var record in recordsToRemove)
        {
            records.InvalidRecords.Add($"{record} - Invalid account number");
            records.ValidRecords.Remove(record);
        }
       
    }
}