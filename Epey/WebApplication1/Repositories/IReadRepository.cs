using WebApplication1.Models;

namespace WebApplication1.Repositories
{
	public interface IReadRepository<T> : IRepository<T> where T : BaseEntity
	{
		IQueryable<T> GetAll();
		Task<T> GetByIdAsync(int id);

	}
}
