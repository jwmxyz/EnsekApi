using System.Linq.Expressions;
using Ensek.DataAccess.DbModels;
using Microsoft.EntityFrameworkCore;

namespace Ensek.DataAccess.Repositories;

public class MeterReadingRepository : IRepository<MeterReading>
{
    private readonly EnsekDbContext _dbContext;

    public MeterReadingRepository(EnsekDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Insert(IEnumerable<MeterReading> entities)
    {
        await _dbContext.MeterReadings.AddRangeAsync(entities);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<MeterReading>> GetAll(Expression<Func<MeterReading, bool>> predicate)
    {
        return await _dbContext.MeterReadings.Where(predicate).ToListAsync();
    }

    public async Task<List<MeterReading>> GetAll()
    {
        return await _dbContext.MeterReadings.AsQueryable().ToListAsync();
    }
}