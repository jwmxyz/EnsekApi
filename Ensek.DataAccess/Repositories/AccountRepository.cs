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

    public Task Insert(IEnumerable<Account> entity)
    {
        throw new NotSupportedException();
    }

    public async Task<List<Account>> GetAll(Expression<Func<Account, bool>> predicate)
    {
        return await _dbContext.Accounts.Where(predicate).ToListAsync();
    }

    public async Task<List<Account>> GetAll()
    {
        return await _dbContext.Accounts.ToListAsync();
    }
}