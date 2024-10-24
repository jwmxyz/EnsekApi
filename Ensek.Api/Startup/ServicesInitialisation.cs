using System.Reflection;
using Ensek.Api.Factory;
using Ensek.Api.Filters;
using Ensek.DataAccess;
using Ensek.DataAccess.DbModels;
using Ensek.DataAccess.Repositories;
using Ensek.ErrorManagement.Exceptions;
using Ensek.Services;
using Ensek.Services.Models.DTO;
using Microsoft.EntityFrameworkCore;
using Account = Ensek.DataAccess.DbModels.Account;

namespace Ensek.Api.Startup;

public static class ServicesInitialisation
{
    public static void RegisterApplicationServices(this IServiceCollection services, WebApplicationBuilder builder) {
        RegisterCustomDependencies(services);
        RegisterDatabaseContextDependencies(services, builder);
    }
    
    private static void RegisterDatabaseContextDependencies(IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddDbContext<EnsekDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    }
    
    private static void RegisterCustomDependencies(IServiceCollection services)
    {
        services
            .AddScoped<ICsvServices, CsvServices>()
            .AddScoped<IRepository<MeterReading>, MeterReadingRepository>()
            .AddScoped<IRepository<Account>, AccountRepository>()
            .AddScoped<IUploadServices<MeterReadingRecord>, MeterUploadServices>()
            .AddScoped<IResponseFactory, ResponseFactory>()
            .AddScoped<IFileValidationService<InvalidCsvException>, CsvFileValidationService<InvalidCsvException>>()
            .AddControllers(opt =>
            {
                opt.Filters.Add(typeof(EnsekExceptionFilter));
            });
    }
}