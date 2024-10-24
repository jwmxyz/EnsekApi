using Ensek.Services.Models.DTO;

namespace Ensek.Services;

/// <summary>
/// A custom IEqualityComparer to compare two MeterReading records.
/// </summary>
public class MeterReadingRecordComparer : IEqualityComparer<MeterReadingRecord>
{
    public bool Equals(MeterReadingRecord? x, MeterReadingRecord? y)
    {
        if (x == null || y == null) return false;
        return x.AccountId == y.AccountId &&
               x.ReadingDateTime == y.ReadingDateTime &&
               x.ReadingValue == y.ReadingValue;
    }

    public int GetHashCode(MeterReadingRecord obj)
    {
        return HashCode.Combine(obj.AccountId, obj.ReadingDateTime, obj.ReadingValue);
    }
}