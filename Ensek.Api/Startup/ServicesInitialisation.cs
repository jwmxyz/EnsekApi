using Ensek.Api.Factory;
using Ensek.Api.Filters;
using Ensek.DataAccess.DbModels;
using Ensek.ErrorManagement.Exceptions;
using Ensek.Services;

namespace Ensek.Api.Startup;

public static class ServicesInitialisation
{
    public static void RegisterApplicationServices(this IServiceCollection services, WebApplicationBuilder builder) {
        RegisterCustomDependencies(services);
    }
    
    private static void RegisterCustomDependencies(IServiceCollection services)
    {
        services
            .AddScoped<ICsvServices, CsvServices>()
            .AddScoped<IUploadServices<MeterReadingRecord>, MeterUploadServices>()
            .AddScoped<IResponseFactory, ResponseFactory>()
            .AddScoped<IFileValidationService<InvalidCsvException>, CsvFileValidationService<InvalidCsvException>>()
            .AddControllers(opt =>
            {
                opt.Filters.Add(typeof(EnsekExceptionFilter));
            });
    }
}