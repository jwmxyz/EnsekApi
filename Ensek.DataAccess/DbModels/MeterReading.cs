
namespace Ensek.DataAccess.DbModels;

public class MeterReading : EntityObject
{
    public int AccountId { get; set; }
    public DateTimeOffset ReadingDateTime { get; set; }
    public uint ReadingValue { get; set; }
    
}