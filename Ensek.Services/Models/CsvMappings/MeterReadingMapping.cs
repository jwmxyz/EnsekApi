using CsvHelper.Configuration;
using Ensek.DataAccess.DbModels;

namespace Ensek.Services.Models.CsvMappings;
public sealed class MeterReadingMapping  : ClassMap<MeterReadingRecord>
{
    private MeterReadingMapping()
    {
        Map(model => model.AccountId).Name("AccountId");
        Map(model => model.DateTime).Name("MeterReadingDateTime")
            .TypeConverter<CsvHelper.TypeConversion.DateTimeOffsetConverter>().TypeConverterOption.Format("dd/MM/yyyy HH:mm");
        Map(model => model.Value).Name("MeterReadValue")
            .Validate(args => 
            {
                if (uint.TryParse(args.Field, out var value))
                {
                    return value is <= 99999;
                }
                return false;
            });
    }
}