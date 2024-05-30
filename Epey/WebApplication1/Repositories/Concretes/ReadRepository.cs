using Microsoft.EntityFrameworkCore;
using WebApplication1.Contexts;
using WebApplication1.Models;

namespace WebApplication1.Repositories.Concrete
{
	public class ReadRepository<T> : IReadRepository<T> where T : BaseEntity
	{
		private readonly EpeyDbContext _context;

		public ReadRepository(EpeyDbContext context)
		{
			_context = context;
		}

		public DbSet<T> Table => _context.Set<T>();

		public IQueryable<T> GetAll()
		{
			var query = Table.AsQueryable();
		
			return query;
		}

		public async Task<T> GetByIdAsync(int id)
		{

			var query = Table.AsQueryable();
		
			return await query.FirstOrDefaultAsync(x => x.Id == id);

		}
	}
}
	