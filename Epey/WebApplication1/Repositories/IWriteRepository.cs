using WebApplication1.Models;

namespace WebApplication1.Repositories
{
	public interface IWriteRepository<T> : IRepository<T> where T : BaseEntity
	{
		Task<bool> AddAsync(T entity);

		bool Remove(T entity);
		Task<bool> RemoveAsync(int id);

		bool RemoveRange(List<T> entities);

		bool Update(T entity);

		Task<int> SaveAsync();


	}
}
