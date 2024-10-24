using CsvHelper.Configuration;
using Ensek.DataAccess.DbModels;

namespace Ensek.DataAccess.Seeding;

public class AccountMapping: ClassMap<Account>
{
    public AccountMapping()
    {
        Map(model => model.Id).Name("AccountId");
        Map(model => model.FirstName).Name("FirstName");
        Map(model => model.LastName).Name("LastName");
    }
}