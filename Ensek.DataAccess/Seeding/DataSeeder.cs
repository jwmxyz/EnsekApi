using System.Globalization;
using CsvHelper;
using Ensek.DataAccess.DbModels;
using Microsoft.EntityFrameworkCore;

namespace Ensek.DataAccess.Seeding;

public static class DataSeeder
{
    public static void SeedAccounts(ModelBuilder modelBuilder)
    {
        using var reader = new StreamReader($"{AppDomain.CurrentDomain.BaseDirectory}/Seeding/SeedData/AccountSeedData.csv");
        using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
        csvReader.Context.RegisterClassMap<AccountMapping>();

        // no checks here as we trust it.
        var accounts = csvReader.GetRecords<Account>();
        modelBuilder.Entity<Account>()
            .HasData(accounts);
    }
}     