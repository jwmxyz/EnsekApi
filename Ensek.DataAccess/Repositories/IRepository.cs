using System.Linq.Expressions;

namespace Ensek.DataAccess.Repositories;

public interface IRepository<TEntity>
{
    /// <summary>
    /// Used to insert IEnumerable{TEntity}
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task Insert(IEnumerable<TEntity> entity);
    
    /// <summary>
    /// Used to GetAll TEntity filtered based on a predicate
    /// </summary>
    /// <param name="predicate">The filtering mechanism</param>
    /// <returns>A List of TEntity based on </returns>
    Task<List<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicate);
    
    /// <summary>
    /// Used to GetAll TEntity
    /// </summary>
    /// <returns>A List of TEntity based on </returns>
    Task<List<TEntity>> GetAll();

}