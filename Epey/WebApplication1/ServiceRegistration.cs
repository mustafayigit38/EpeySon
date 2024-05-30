using WebApplication1.Contexts;
using WebApplication1.Models;
using WebApplication1.Repositories.Concrete;
using WebApplication1.Repositories;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1
{
	public static class ServiceRegistration
	{
		public static void AddPersistenceServices(this IServiceCollection services)
		{
			services.AddDbContext<EpeyDbContext>(
			   options => options.UseSqlite("Data Source=.\\data\\epeydb.db")
			);

			services.AddScoped<IWriteRepository<Category>, WriteRepository<Category>>();
			services.AddScoped<IReadRepository<Category>, ReadRepository<Category>>();

			services.AddScoped<IWriteRepository<Brand>, WriteRepository<Brand>>();
            services.AddScoped<IReadRepository<Brand>, ReadRepository<Brand>>();

            services.AddScoped<IWriteRepository<Product>, WriteRepository<Product>>();
            services.AddScoped<IReadRepository<Product>, ReadRepository<Product>>();

			services.AddScoped<IWriteRepository<User>, WriteRepository<User>>();
            services.AddScoped<IReadRepository<User>, ReadRepository<User>>();

            services.AddScoped<IWriteRepository<Phone>, WriteRepository<Phone>>();
            services.AddScoped<IReadRepository<Phone>, ReadRepository<Phone>>();



				

        }
	}
}