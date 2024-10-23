namespace Ensek.DataAccess.DbModels;

public class MeterReadingRecord
{
    public int AccountId { get; set; }
    public DateTimeOffset DateTime { get; set; }
    public uint Value { get; set; }
}