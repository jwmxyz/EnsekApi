using System.Linq.Expressions;

namespace Ensek.DataAccess.Repositories;

public interface IRepository<TEntity>
{
    Task Insert(IEnumerable<TEntity> entity);
    Task<List<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicate);
    Task<List<TEntity>> GetAll();

}