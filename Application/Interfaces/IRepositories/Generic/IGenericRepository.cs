using System.Linq.Expressions;

namespace Application.Inerfaces.IRepositories.Generic
{
	public interface IGenericRepository<T> where T : class
	{
		// For chaining and advanced querying
		IQueryable<T> GetAllQueryable(string? includeProperties = null, bool tracked = false); // IQueryable<T>

		// For immediate data retrieval
		Task<IEnumerable<T>> GetAllAsync(string? includeProperties = null, bool tracked = false); // Task<IEnumerable<T>>

		// Same for filtered queries
		IQueryable<T> FindAllQueryable(Expression<Func<T, bool>> predicate, string? includeProperties = null, bool tracked = false); // IQueryable<T>

		Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate, string? includeProperties = null, bool tracked = false); // Task<IEnumerable<T>>

		Task<T?> GetByIdAsync(int id, string? includeProperties = null, bool tracked = false);
		Task<T?> GetAsync(Expression<Func<T, bool>> predicate, string? includeProperties = null, bool tracked = false);
		Task AddAsync(T entity);
		void Remove(T entity);
		void RemoveRange(IEnumerable<T> entities);
	}

}
