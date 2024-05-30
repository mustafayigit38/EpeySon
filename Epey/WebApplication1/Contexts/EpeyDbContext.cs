using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Contexts
{
	public class EpeyDbContext: DbContext
	{
        public EpeyDbContext(DbContextOptions options) : base(options)
		{
            
        }
        public DbSet<User> Users { get; set; }

        public DbSet<Category> Categories { get; set; }

		public DbSet<Phone> Phones { get; set; }

		public DbSet<Brand> Brands { get; set; }






		public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			//  ChangeTracker yeni eklenen veriyi yakalamak için kullanılır.

			var entries = ChangeTracker.Entries<BaseEntity>();

			foreach (var item in entries)
			{
				switch (item.State)
				{
					case EntityState.Added:
						item.Entity.CreatedAt = DateTime.UtcNow;
						break;
					default:
						break;
				}

			}

			return await base.SaveChangesAsync(cancellationToken);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Phone>()
				.OwnsOne(p => p.PhoneBatterySpecs);
			modelBuilder.Entity<Phone>()
				.OwnsOne(p => p.PhoneCameraSpecs);
			modelBuilder.Entity<Phone>()
				.OwnsOne(p => p.PhoneScreenSpecs);
		}
	}
}
