using Ensek.DataAccess.DbModels;
using Ensek.Services.Models;

namespace Ensek.Services;

public interface IUploadServices<T>
{
    Task<CsvParsingResults<T>> Upload(Stream fileStream);
}