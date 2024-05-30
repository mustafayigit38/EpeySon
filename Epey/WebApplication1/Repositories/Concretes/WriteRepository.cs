using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.Contexts;

namespace WebApplication1.Repositories.Concrete
{
	public class WriteRepository<T>: IWriteRepository<T> where T : BaseEntity
	{
		private readonly EpeyDbContext _context;

		public WriteRepository(EpeyDbContext context)
		{
			_context = context;
		}

		public DbSet<T> Table => _context.Set<T>();

		public async Task<bool> AddAsync(T entity)
		{
			EntityEntry<T> entityEntry = await Table.AddAsync(entity);
			return entityEntry.State == EntityState.Added;
		}

		public async Task<bool> AddRangeAsync(List<T> entities)
		{
			await Table.AddRangeAsync(entities);
			return true;
		}

		public bool Remove(T entity)
		{
			EntityEntry<T> entityEntry = Table.Remove(entity);
			return entityEntry.State == EntityState.Deleted;
		}

		public async Task<bool> RemoveAsync(int id)
		{
			T entity = await Table.FirstOrDefaultAsync(x => x.Id ==id);
			return Remove(entity);
		}

		public bool RemoveRange(List<T> entities)
		{
			Table.RemoveRange(entities);
			return true;
		}

		public bool Update(T entity)
		{
			EntityEntry<T> entityEntry = Table.Update(entity);
			return entityEntry.State == EntityState.Modified;
		}

		public async Task<int> SaveAsync() => await _context.SaveChangesAsync();

		
	}
}
