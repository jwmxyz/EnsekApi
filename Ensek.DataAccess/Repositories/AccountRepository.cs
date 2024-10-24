using System.Linq.Expressions;
using Ensek.DataAccess.DbModels;
using Microsoft.EntityFrameworkCore;

namespace Ensek.DataAccess.Repositories;

public class AccountRepository : IRepository<Account>
{
    private readonly EnsekDbContext _dbContext;

    public AccountRepository(EnsekDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc cref="IRepository{TEntity}.Insert"/>
    public Task Insert(IEnumerable<Account> entity)
    {
        // breaks SOLID atm but normally this would be used...
        throw new NotSupportedException();
    }
    
    /// <inheritdoc cref="IRepository{TEntity}.GetAll(System.Linq.Expressions.Expression{System.Func{TEntity,bool}})"/>
    public async Task<List<Account>> GetAll(Expression<Func<Account, bool>> predicate)
    {
        return await _dbContext.Accounts.Where(predicate).ToListAsync();
    }

    /// <inheritdoc cref="IRepository{TEntity}.GetAll()"/>
    public async Task<List<Account>> GetAll()
    {
        return await _dbContext.Accounts.ToListAsync();
    }
}