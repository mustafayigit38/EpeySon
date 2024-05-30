using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
	public interface IRepository<T> where T : BaseEntity
	{
		DbSet<T> Table { get; }
	}
}
