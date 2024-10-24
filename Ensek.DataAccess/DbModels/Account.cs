namespace Ensek.DataAccess.DbModels;

public class Account : EntityObject
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public virtual ICollection<MeterReading>? MeterReadings { get; set; }
}