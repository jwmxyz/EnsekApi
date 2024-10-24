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

    /// <inheritdoc cref="IRepository{TEntity}.Insert"/>
    public async Task Insert(IEnumerable<MeterReading> entities)
    {
        //todo better error management
        await _dbContext.MeterReadings.AddRangeAsync(entities);
        await _dbContext.SaveChangesAsync();
    }

    /// <inheritdoc cref="IRepository{TEntity}.GetAll(System.Linq.Expressions.Expression{System.Func{TEntity,bool}})"/>
    public async Task<List<MeterReading>> GetAll(Expression<Func<MeterReading, bool>> predicate)
    {
        return await _dbContext.MeterReadings.Where(predicate).ToListAsync();
    }

    /// <inheritdoc cref="IRepository{TEntity}.GetAll()"/>
    public async Task<List<MeterReading>> GetAll()
    {
        return await _dbContext.MeterReadings.AsQueryable().ToListAsync();
    }
}