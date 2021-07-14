namespace MeterReadingsData
{
	using MeterReadingsData.Models;
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
		}

		public DbSet<Account> Accounts { get; set; }

		public DbSet<MeterReading> MeterReadings { get; set; }
	}
}