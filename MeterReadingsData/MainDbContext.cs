namespace MeterReadingsData
{
	using MeterReadingsData.Entities;
	using Microsoft.EntityFrameworkCore;

	public class MainDbContext : DbContext
	{
		public MainDbContext(DbContextOptions options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Account>()
				.HasMany(m => m.MeterReadings)
				.WithOne(m => m.MyAccount)
				.HasForeignKey(m => m.AccountId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Account>()
				.Property(e => e.Id)
				.ValueGeneratedOnAdd();

			modelBuilder.Entity<MeterReading>()
				.Property(e => e.Id)
				.ValueGeneratedOnAdd();
		}

		public DbSet<Account> Accounts { get; set; }

		public DbSet<MeterReading> MeterReadings { get; set; }
	}
}