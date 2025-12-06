using Application.Inerfaces.IRepositories.Generic;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories.Generic
{
	public class GenericRepository<T> : IGenericRepository<T> where T : class
	{
		private readonly AppDbContext _context;
		private readonly DbSet<T> _dbSet;

		public GenericRepository(AppDbContext context)
		{
			_context = context;
			_dbSet = _context.Set<T>();
		}

		// For advanced querying with IQueryable
		public IQueryable<T> GetAllQueryable(string? includeProperties = null, bool tracked = false)
		{
			IQueryable<T> query = _dbSet;

			if (!tracked)
				query = query.AsNoTracking();

			query = IncludeNavigationProperties(query, includeProperties);
			return query;
		}

		// For immediate data retrieval
		public async Task<IEnumerable<T>> GetAllAsync(string? includeProperties = null, bool tracked = false)
		{
			IQueryable<T> query = _dbSet;

			if (!tracked)
				query = query.AsNoTracking();

			query = IncludeNavigationProperties(query, includeProperties);
			return await query.ToListAsync();
		}

		// For filtering with IQueryable
		public IQueryable<T> FindAllQueryable(Expression<Func<T, bool>> predicate, string? includeProperties = null, bool tracked = false)
		{
			IQueryable<T> query = _dbSet;

			if (!tracked)
				query = query.AsNoTracking();

			query = query.Where(predicate);
			query = IncludeNavigationProperties(query, includeProperties);
			return query;
		}

		// For immediate filtered data retrieval
		public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate, string? includeProperties = null, bool tracked = false)
		{
			IQueryable<T> query = _dbSet;

			if (!tracked)
				query = query.AsNoTracking();  // Apply AsNoTracking before the filter

			query = query.Where(predicate);  // Apply the filter
			query = IncludeNavigationProperties(query, includeProperties);  // Include any related entities

			return await query.ToListAsync();
		}

		public async Task<T?> GetByIdAsync(int id, string? includeProperties = null, bool tracked = false)
		{
			if (tracked && string.IsNullOrEmpty(includeProperties))
			{
				return await _dbSet.FindAsync(id);
			}

			IQueryable<T> query = _dbSet;

			if (!tracked)
				query = query.AsNoTracking();

			query = IncludeNavigationProperties(query, includeProperties);
			return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
		}

		// For retrieving a single entity using a custom predicate
		public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate, string? includeProperties = null, bool tracked = false)
		{
			IQueryable<T> query = _dbSet;

			if (!tracked)
				query = query.AsNoTracking();

			query = IncludeNavigationProperties(query, includeProperties);
			return await query.FirstOrDefaultAsync(predicate);
		}

		public async Task AddAsync(T entity)
		{
			await _dbSet.AddAsync(entity);
		}

		public void Remove(T entity)
		{
			_dbSet.Remove(entity);
		}

		public void RemoveRange(IEnumerable<T> entities)
		{
			_dbSet.RemoveRange(entities);
		}

		// Helper method for including navigation properties
		private IQueryable<T> IncludeNavigationProperties(IQueryable<T> query, string? includeProperties = null)
		{
			if (!string.IsNullOrWhiteSpace(includeProperties))
			{
				var properties = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				foreach (var property in properties)
				{
					query = query.Include(property);
				}
			}
			return query;
		}
	}
}

