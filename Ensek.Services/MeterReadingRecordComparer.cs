using Ensek.Services.Models.DTO;

namespace Ensek.Services;

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