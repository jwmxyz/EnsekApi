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

    /// <inheritdoc cref="IUploadServices{T}.Upload"/>
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

    /// <summary>
    /// Method used to remove duplicate entries that exist in both the csv file and the existing dataset.
    /// With large datasets this would not be a good approach.
    /// We could instead load the data into a seperate table with nonclustered indexes applied
    /// We could then find any duplicates and remove those before commiting the entire tale
    /// </summary>
    /// <param name="records">The current record collection.</param>
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

    /// <summary>
    /// Method used to remove any invalid accounts that have tried to be passed in
    /// I would refrain from loading everything into memory for each .csv. Instead I could build a
    /// cache of accounts before any processing is completed and verify the accounts exists. 
    /// </summary>
    /// <param name="records">The current record collection</param>
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
            .ToList(); 

        foreach (var record in recordsToRemove)
        {
            records.InvalidRecords.Add($"{record} - Invalid account number");
            records.ValidRecords.Remove(record);
        }
    }
}