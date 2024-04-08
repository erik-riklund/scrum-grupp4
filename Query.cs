using MongoDB.Driver;
using MongoDB.Entities;
using System.Linq.Expressions;

namespace App
{
  public static class Query
  {
    /// <summary>Fetch all entities of the specified type.</summary>
    /// <returns>IEnumerable that can be iterated once.</returns>
    public static async Task<IEnumerable<TEntity>> FetchAll<TEntity>() where TEntity : Entity
    {
      return (await DB.Find<TEntity>().ExecuteCursorAsync()).ToEnumerable();
    }

    /// <summary>Fetch that entities that match the specified expression.</summary>
    /// <param name="expression">LINQ expression specifying the match criteria.</param>
    /// <param name="limit">Limit the number of entities returned as part of the result.</param>
    /// <returns>IEnumerable that can be iterated once.</returns>
    public static async Task<IEnumerable<IEntity>> FetchMany<TEntity>(Expression<Func<TEntity, bool>> expression, int limit = 99999) where TEntity : Entity
    {
      return await DB.Find<TEntity>().Limit(limit).ManyAsync(expression);
    }

    /// <summary>Fetch a single entity with the specified ID.</summary>
    /// <param name="id">ObjectID as a string.</param>
    /// <returns>TEntity, or null if no entity match the ID.</returns>
    public static async Task<TEntity?> FetchOneById<TEntity>(string id) where TEntity : Entity
    {
      return await DB.Find<TEntity>().OneAsync(id);
    }

    /// <summary>Fetch the first entity that match the specified expression.</summary>
    /// <param name="expression">LINQ expression specifying the match criteria.</param>
    /// <returns>TEntity, or null if no entity match the ID.</returns>
    public static async Task<TEntity?> FetchOne<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : Entity
    {
      var result = await DB.Find<TEntity>().Limit(1).ManyAsync(expression);

      return result[0] ?? null;
    }
  }
}
