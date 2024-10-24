using Ensek.DataAccess.DbModels;

namespace Ensek.Services.Models.DTO;

public class MeterReadingRecord
{
    public int AccountId { get; set; }
    public DateTimeOffset ReadingDateTime { get; set; }
    public uint ReadingValue { get; set; }

    public override string ToString()
    {
        return $"{AccountId}, {ReadingDateTime:dd/MM/yyyy HH:mm}, {ReadingValue}";
    }
    
    public override bool Equals(object obj)
    {
        if (obj is MeterReading other)
        {
            return AccountId == other.AccountId && ReadingDateTime == other.ReadingDateTime &&
                   ReadingValue == other.ReadingValue;
        }
        if (obj is MeterReadingRecord mrr)
        {
            return AccountId == mrr.AccountId &&
                   ReadingDateTime == mrr.ReadingDateTime &&
                   ReadingValue == mrr.ReadingValue;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(AccountId, ReadingDateTime, ReadingValue);
    }
}